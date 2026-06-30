import { useMemo } from "react";
import { useStore } from "@tanstack/react-form";

type UnknownFieldError =
  | string
  | string[]
  | {
      errors?: unknown[];
      errorMap?: Record<string, unknown>;
    }
  | undefined
  | null;

type FieldWithErrorState = {
  name: string;
  state: {
    meta: {
      errors: unknown[];
      isTouched: boolean;
    };
  };
  form?: {
    state?: {
      submissionAttempts?: number;
      errorMap?: unknown;
      fieldMeta?: Record<string, unknown>;
    };
    store?: unknown;
    getAllErrors?: () => {
      fields?: Record<string, UnknownFieldError>;
    };
  };
};

function normalizeErrors(errors: unknown): string[] {
  if (!errors) {
    return [];
  }

  if (typeof errors === "string") {
    return errors.trim() ? [errors] : [];
  }

  if (Array.isArray(errors)) {
    return errors.flatMap(normalizeErrors);
  }

  if (typeof errors === "object") {
    const candidate = errors as {
      errors?: unknown;
      errorMap?: Record<string, unknown>;
    };

    return [
      ...normalizeErrors(candidate.errors),
      ...normalizeErrors(
        candidate.errorMap ? Object.values(candidate.errorMap) : undefined,
      ),
    ];
  }

  return [String(errors)];
}

function uniqueErrors(errors: string[]) {
  return Array.from(new Set(errors.map((error) => error.trim()).filter(Boolean)));
}

export function getFieldErrors(field: FieldWithErrorState) {
  const fieldLevelErrors = normalizeErrors(field.state.meta.errors);
  const allErrors = field.form?.getAllErrors?.();
  const formLevelErrors = normalizeErrors(allErrors?.fields?.[field.name]);

  return uniqueErrors([...fieldLevelErrors, ...formLevelErrors]);
}

export function shouldShowFieldErrors(field: FieldWithErrorState) {
  const submissionAttempts = field.form?.state?.submissionAttempts ?? 0;
  const errors = getFieldErrors(field);

  return errors.length > 0 && (field.state.meta.isTouched || submissionAttempts > 0);
}

export function useFieldValidationState(field: FieldWithErrorState) {
  const formState = useStore(field.form!.store as any, (state: any) => ({
    submissionAttempts: state.submissionAttempts ?? 0,
    errorMap: state.errorMap,
    fieldMeta: state.fieldMeta?.[field.name],
  }));

  const errors = useMemo(
    () => getFieldErrors(field),
    [field, field.name, field.state.meta.errors, formState.errorMap, formState.fieldMeta],
  );

  return {
    errors,
    showError:
      errors.length > 0 &&
      (field.state.meta.isTouched || formState.submissionAttempts > 0),
  };
}
