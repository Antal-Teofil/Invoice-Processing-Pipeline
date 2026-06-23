import { useFieldContext } from "../../../lib/form-context";

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

  return (
    <label>
      <span>{label}</span>

      <select
        value={field.state.value}
        onBlur={field.handleBlur}
        onChange={(event) => {
          field.handleChange(event.target.value);
        }}
      >
        {options.map((option) => (
          <option key={option.code} value={option.code} title={option.label}>
            {option.code}
          </option>
        ))}
      </select>
    </label>
  );
}
