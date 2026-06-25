import { useFieldContext } from "../../../lib/form-context";

export function NumberField({ label }: { label: string }) {
  const field = useFieldContext<string>();

  return (
    <label className="text-form-field">
      <span>{label}</span>

      <input
        type="number"
        value={field.state.value ?? ""}
        onBlur={field.handleBlur}
        onChange={(e) => {
          field.handleChange(e.target.value);
        }}
      />
    </label>
  );
}
