import type { InvoiceSummaryCard } from "../types/InvoiceSummaryCard";


export default function InvoiceSummaryCardComponent(invoice: InvoiceSummaryCard) {
    return (
        <tr>
            <td>{invoice.invoiceId}</td>
            <td>{invoice.vendor}</td>
            <td>{invoice.vendorEmailAddress}</td>
            <td>{invoice.phoneNumber}</td>
            <td>{invoice.totalAmount}</td>
            <td>{invoice.currencyCode}</td>
            <td>{invoice.status}</td>
        </tr>
    );
}