import { useFieldContext } from "../../../lib/form-context";
import { FieldErrors, getFieldErrorDialogId } from "./FieldErrors";
import { useFieldValidationState } from "./field-error-visibility";

type EmailFieldProps = {
  label: string;
  placeholder?: string;
};

export function EmailField({ label, placeholder }: EmailFieldProps) {
  const field = useFieldContext<string>();
  const { errors, showError } = useFieldValidationState(field);
  const errorDialogId = getFieldErrorDialogId(field.name);

  return (
    <label className="text-form-field">
      <span>{label}</span>

      <input
        type="email"
        name={field.name}
        placeholder={placeholder}
        value={field.state.value ?? ""}
        aria-invalid={showError}
        aria-describedby={showError ? errorDialogId : undefined}
        onBlur={field.handleBlur}
        onChange={(event) => {
          field.handleChange(event.target.value);
        }}
      />

      <FieldErrors
        show={showError}
        errors={errors}
        fieldName={field.name}
      />
    </label>
  );
}
