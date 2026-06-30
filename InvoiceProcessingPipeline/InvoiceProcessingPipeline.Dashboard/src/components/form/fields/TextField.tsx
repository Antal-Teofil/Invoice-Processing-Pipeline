import { useFieldContext } from "../../../lib/form-context";
import { FieldErrors, getFieldErrorDialogId } from "./FieldErrors";
import { useFieldValidationState } from "./field-error-visibility";

export default function TextField({ label }: { label: string }) {
  const field = useFieldContext<string>();
  const { errors, showError } = useFieldValidationState(field);
  const errorDialogId = getFieldErrorDialogId(field.name);

  return (
    <label className="text-form-field">
      <span>{label}</span>
      <input
        type="text"
        name={field.name}
        value={field.state.value ?? ""}
        aria-invalid={showError}
        aria-describedby={showError ? errorDialogId : undefined}
        onBlur={field.handleBlur}
        onChange={(e) => field.handleChange(e.target.value)}
      />

      <FieldErrors
        show={showError}
        errors={errors}
        fieldName={field.name}
      />
    </label>
  );
}
