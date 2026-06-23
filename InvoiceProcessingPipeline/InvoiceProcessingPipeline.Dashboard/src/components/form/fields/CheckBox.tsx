import { useFieldContext } from "../../../lib/form-context";

type CheckboxFieldProps = {
  label: string;
};

export function CheckboxField({ label }: CheckboxFieldProps) {
  const field = useFieldContext<boolean>();

  return (
    <label>
      <input
        type="checkbox"
        checked={field.state.value ?? false}
        onBlur={field.handleBlur}
        onChange={(event) => {
          field.handleChange(event.target.checked);
        }}
      />

      <span>{label}</span>
    </label>
  );
}