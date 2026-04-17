import type { InvoiceSummaryRecord } from "../types/InvoiceSummaryRecord";


export default function InvoiceSummaryRecordRow({
    invoiceId, 
    vendor, 
    vendorEmailAddress,
    phoneNumber,
    totalAmount,
    currencyCode,
    status,
}: InvoiceSummaryRecord) {
    return (
        <tr>
            <td>{invoiceId}</td>
            <td>{vendor}</td>
            <td>{vendorEmailAddress}</td>
            <td>{phoneNumber}</td>
            <td>{totalAmount}</td>
            <td>{currencyCode}</td>
            <td>{status}</td>
        </tr>
    );
}