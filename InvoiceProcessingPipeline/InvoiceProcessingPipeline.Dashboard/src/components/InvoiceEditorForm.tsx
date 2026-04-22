import { useForm } from "@tanstack/react-form"
import type Invoice from "../types/InvoiceScheme";

export default function InvoiceEditorForm( {} : Invoice) {

    const invoiceForm = useForm({

    });

    return (
        <div>
            <h1>INVOICE</h1>
            <form>
            </form>
        </div>
    );
}
