import { useFieldContext } from "../../../lib/form-context";

export default function TextField({ label }: { label: string }) {
  const field = useFieldContext<string>();

  return (
    <label className="text-form-field">
      <span>{label}</span>
      <input
        type="text"
        value={field.state.value ?? ""}
        onBlur={field.handleBlur}
        onChange={(e) => field.handleChange(e.target.value)}
      />
    </label>
  );
}
