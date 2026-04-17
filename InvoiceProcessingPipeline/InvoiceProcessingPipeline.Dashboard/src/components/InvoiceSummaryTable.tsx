import { useInvoiceSummaryRecords } from "../hooks/use-invoice-records";
import InvoiceSummaryRecordRow from "./InvoiceSummaryRecordRow";

export default function InvoiceSummaryTable() {

    const {data, isPending, isError, error} = useInvoiceSummaryRecords();

    if (isPending) {
        return <div>Loading...</div>
    }

    if (isError) {
        return <div>Error: {error.message}</div>
    }

    const records = data ?? [];
    return (
        <div>
            <table>
                <caption>
                    Invoice List
                </caption>

                <thead>
                    <tr>
                        <th scope="col">ID</th>
                        <th scope="col">VENDOR NAME</th>
                        <th scope="col">E-MAIL ADDRESS</th>
                        <th scope="col">PHONE NUMBER</th>
                        <th scope="col">TOTAL</th>
                        <th scope="col">CURRENCY</th>
                        <th scope="col">STATUS</th>
                    </tr>
                </thead>
                <tbody>
                    {records.length === 0 ? (
                    <tr>
                        <td colSpan={7}>No invoices found.</td>
                    </tr>
                    ) : (
                    records.map((record) => (
                        <InvoiceSummaryRecordRow key={record.invoiceId} {...record} />
                    ))
                    )}
                </tbody>
            </table>
        </div>
    );
}