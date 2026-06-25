import { useFieldContext } from "../../../lib/form-context";

type EmailFieldProps = {
  label: string;
  placeholder?: string;
};

export function EmailField({ label }: EmailFieldProps) {
  const field = useFieldContext<string>();

  return (
    <label className="text-form-field">
      <span>{label}</span>

      <input
        type="email"
        value={field.state.value ?? ""}
        onBlur={field.handleBlur}
        onChange={(event) => {
          field.handleChange(event.target.value);
        }}
      />
    </label>
  );
}