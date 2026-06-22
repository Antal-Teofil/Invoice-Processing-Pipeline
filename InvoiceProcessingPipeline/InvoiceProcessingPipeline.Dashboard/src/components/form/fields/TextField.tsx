import { useFieldContext } from "../../../lib/form-context";

export default function TextFiled({ label } : { label : string}) {
    
    const field = useFieldContext<string>();

    return (
        <div>
            <label htmlFor={field.name}>{label}</label>

            <input 
            id={field.name}
            name={field.name}
            value={field.state.value ?? ""}
            />
        </div>
    );
};