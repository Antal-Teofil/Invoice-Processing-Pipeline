import { useInvoiceData } from "../hooks/use-invoice";
import { useForm } from '@tanstack/react-form'
import { CURRENCY_CODES, EU_RELEVANT_CURRENCY_CODES, INVOICE_TYPE_CODES } from "../types/invoice-type-codes";

export function InvoiceEditorForm( invoiceId: string ) {

    const { data, isLoading, error } = useInvoiceData(invoiceId);

    if(isLoading){
        return <div>Loading Form...</div>
    }
    if(error) {
        return <div>Error: {error.message}</div>
    }
    if(!data) {
        return <div>No Invoice Found</div>
    }

    const form = useForm({
        defaultValues: { data },
    });

    return (

        <div>
            <form>
                <h1>INVOICE</h1>
                <div className="invoice-info">
                    <form.Field
                        name="data.invoiceId" // INVOICE ID
                        children={(field) => (
                            <div>
                                <label>Invoice Identifier:</label>
                                <input
                                    value={field.state.value}
                                    onChange={(e) => field.handleChange(e.target.value)}
                                    onBlur={field.handleBlur}
                                />
                            </div>
                        )}
                    />

                    <form.Field
                        name="data.typeCode" // INVOICE TYPE CODE
                        children={(field) => (
                            <div>
                                <label>Invoice Type Code:</label>
                                <select
                                    value={field.state.value}
                                    onChange={(e) => field.handleChange(e.target.value)}
                                    onBlur={field.handleBlur}
                                >
                                    {
                                        INVOICE_TYPE_CODES.map((typeCode) => (
                                            <option key={typeCode.code} value={typeCode.}>
                                                {typeCode.label}
                                            </option>
                                        ))
                                    }
                                </select>
                            </div>
                        )}
                    />
                    <form.Field
                        name="data.note" // INVOICE NOTE
                        children={(field) => (
                            <div>
                                <label>Invoice Note:</label>
                                <input
                                    value={field.state.value}
                                    onChange={(e) => field.handleChange(e.target.value)}
                                    onBlur={field.handleBlur}
                                />
                            </div>
                        )}
                    />

                    <form.Field
                        name="data.issueDate" // ISSUE DATE
                        children={(field) => (
                            <div>
                                <label>Issue Date:</label>
                                <input
                                    type="date"
                                    value={field.state.value}
                                    onChange={(e) => field.handleChange(e.target.value)}
                                    onBlur={field.handleBlur}
                                />
                            </div>
                        )}
                    />

                    <form.Field
                        name="data.dueDate" // DUE DATE
                        children={(field) => (
                            <div>
                                <label>Due Date:</label>
                                <input
                                    type="date"
                                    value={field.state.value}
                                    onChange={(e) => field.handleChange(e.target.value)}
                                    onBlur={field.handleBlur}
                                />
                            </div>
                        )}
                    />

                    <form.Field
                        name="data.taxPointDate" // TAX POINT DATE
                        children={(field) => (
                            <div>
                                <label>Tax Point Date:</label>
                                <input
                                    type="date"
                                    value={field.state.value}
                                    onChange={(e) => field.handleChange(e.target.value)}
                                    onBlur={field.handleBlur}
                                />
                            </div>
                        )}
                    />
                    
                    <form.Field
                        name="data.taxCurrencyCode" // INVOICE TYPE CODE
                        children={(field) => (
                            <div>
                                <label>Tax Currency Code:</label>
                                <select
                                    value={field.state.value}
                                    onChange={(e) => field.handleChange(e.target.value)}
                                    onBlur={field.handleBlur}
                                >
                                    {
                                        CURRENCY_CODES.map((typeCode) => (
                                            <option key={typeCode.code} value={typeCode.}>
                                                {`${typeCode.code} - ${typeCode.label}`}
                                            </option>
                                        ))
                                    }
                                </select>
                            </div>
                        )}
                    />
                </div>
                <div className="customer-party">

                </div>
                <div className="vendor-party">

                </div>
                <div className="invoice-lines">

                </div>
            </form>
        </div>
    );
};