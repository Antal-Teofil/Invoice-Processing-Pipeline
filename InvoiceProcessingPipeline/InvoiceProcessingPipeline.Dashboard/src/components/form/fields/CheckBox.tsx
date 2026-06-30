import { useFieldContext } from "../../../lib/form-context";
import { FieldErrors, getFieldErrorDialogId } from "./FieldErrors";
import { useFieldValidationState } from "./field-error-visibility";

type CheckboxFieldProps = {
  label: string;
};

export function CheckboxField({ label }: CheckboxFieldProps) {
  const field = useFieldContext<boolean>();
  const { errors, showError } = useFieldValidationState(field);
  const errorDialogId = getFieldErrorDialogId(field.name);

  return (
    <label className="checkbox-form-field">
      <input
        type="checkbox"
        name={field.name}
        checked={field.state.value ?? false}
        aria-invalid={showError}
        aria-describedby={showError ? errorDialogId : undefined}
        onBlur={field.handleBlur}
        onChange={(event) => {
          field.handleChange(event.target.checked);
        }}
      />

      <span>{label}</span>

      <FieldErrors
        show={showError}
        errors={errors}
        fieldName={field.name}
      />
    </label>
  );
}
