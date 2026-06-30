import { useFieldContext } from "../../../lib/form-context";
import { FieldErrors, getFieldErrorDialogId } from "./FieldErrors";
import { useFieldValidationState } from "./field-error-visibility";

type FieldOption = {
  code: string;
  label: string;
};

export default function OptionField({
  label,
  options,
}: {
  label: string;
  options: readonly FieldOption[];
}) {
  const field = useFieldContext<string>();
  const { errors, showError } = useFieldValidationState(field);
  const errorDialogId = getFieldErrorDialogId(field.name);

  return (
    <label className="option-form-field">
      <span>{label}</span>

      <select
        name={field.name}
        value={field.state.value ?? ""}
        aria-invalid={showError}
        aria-describedby={showError ? errorDialogId : undefined}
        onBlur={field.handleBlur}
        onChange={(event) => {
          field.handleChange(event.target.value);
        }}
      >
        <option value="">-</option>

        {options.map((option) => (
          <option key={option.code} value={option.code} title={option.label}>
            {option.code}
          </option>
        ))}
      </select>

      <FieldErrors
        show={showError}
        errors={errors}
        fieldName={field.name}
      />
    </label>
  );
}
