import InvoiceTableRow, { type InvoiceRow } from "./InvoiceTableRow";

type InvoiceTableProperties = {
  invoiceRows: InvoiceRow[];
};

export default function InvoiceTable({ invoiceRows }: InvoiceTableProperties) {
  return (
    <table>
      <thead>
        <tr>
          <th>Invoice ID</th>
          <th>Vendor</th>
          <th>Email</th>
          <th>Phone</th>
          <th>Amount</th>
          <th>Issue Date</th>
        </tr>
      </thead>
      <tbody>
        {invoiceRows.map((row) => (
          <InvoiceTableRow key={row.invoiceId} {...row} />
        ))}
      </tbody>
    </table>
  );
}