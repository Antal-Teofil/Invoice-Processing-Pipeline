import { useFieldContext } from "../../../lib/form-context";

export default function DateField({ label }: { label: string }) {
  const field = useFieldContext<string>();

  return (
    <label className="date-form-field">
      <span>{label}</span>

      <input
        type="date"
        name={field.name}
        value={field.state.value ?? ""}
        onBlur={field.handleBlur}
        onChange={(e) => field.handleChange(e.target.value)}
      />
    </label>
  );
}