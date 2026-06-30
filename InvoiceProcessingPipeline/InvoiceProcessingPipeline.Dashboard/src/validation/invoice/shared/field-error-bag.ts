import type { FieldErrorBag, TanStackFormErrorResult } from "./validator.types";

export function createFieldErrorBag(): FieldErrorBag {
  return {};
}

export function addFieldError(
  errors: FieldErrorBag,
  field: string,
  message: string,
) {
  if (!errors[field]) {
    errors[field] = message;
    return;
  }

  if (!errors[field].includes(message)) {
    errors[field] = `${errors[field]} ${message}`;
  }
}

export function mergeFieldErrors(
  target: FieldErrorBag,
  source: FieldErrorBag,
) {
  Object.entries(source).forEach(([field, message]) => {
    addFieldError(target, field, message);
  });

  return target;
}

export function toTanStackFormError(
  fields: FieldErrorBag,
): TanStackFormErrorResult {
  if (Object.keys(fields).length === 0) {
    return undefined;
  }

  return { fields };
}
