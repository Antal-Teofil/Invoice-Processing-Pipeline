export type InvoiceRow = {
  invoiceId: string;
  vendorName: string;
  emailAddress: string;
  phoneNumber: string;
  amount: number;
  issueDate: string;
};

export default function InvoiceTableRow({
  invoiceId,
  vendorName,
  emailAddress,
  phoneNumber,
  amount,
  issueDate,
}: InvoiceRow) {
  return (
    <tr>
      <td>{invoiceId}</td>
      <td>{vendorName}</td>
      <td>{emailAddress}</td>
      <td>{phoneNumber}</td>
      <td>{amount}</td>
      <td>{issueDate}</td>
    </tr>
  );
}