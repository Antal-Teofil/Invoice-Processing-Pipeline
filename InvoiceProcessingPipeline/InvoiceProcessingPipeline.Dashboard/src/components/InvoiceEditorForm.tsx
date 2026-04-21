import { useForm } from "@tanstack/react-form"
import type Invoice from "../types/InvoiceSchema";

export default function InvoiceEditorForm( {} : Invoice) {

    const invoiceForm = useForm({

    });

    return (
        <div>
            <h1>INVOICE</h1>
            <form>
                <invoiceForm.Field name="">

                </invoiceForm.Field>
            </form>
        </div>
    );
}
