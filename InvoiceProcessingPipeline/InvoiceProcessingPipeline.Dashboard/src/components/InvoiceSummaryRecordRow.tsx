import { useNavigate } from "react-router-dom";
import type { InvoiceSummaryRecord } from "../types/invoice-summary-record";

function formatStatus(status: string) {
  return status.replaceAll("_", " ");
}

function formatAmount(amount: number | null, currencyCode: string | null) {
  if (amount === null) {
    return "-";
  }

  return new Intl.NumberFormat("hu-HU", {
    style: "currency",
    currency: currencyCode ?? "HUF",
  }).format(amount);
}

function formatDate(value: string) {
  return new Intl.DateTimeFormat("hu-HU", {
    dateStyle: "short",
    timeStyle: "short",
  }).format(new Date(value));
}

export default function InvoiceSummaryRecordRow({
  documentId,
  invoiceNumber,
  supplier,
  phoneNumber,
  emailAddress,
  totalAmount,
  currencyCode,
  auditor,
  auditStatus,
  updatedAt,
}: InvoiceSummaryRecord) {
  const navigate = useNavigate();

  const handleRowClick = () => {
    navigate(`/editor/${documentId}`);
  };

  return (
    <tr onClick={handleRowClick}>
      <td>{invoiceNumber ?? documentId}</td>

      <td>{supplier ?? "-"}</td>

      <td>{phoneNumber ?? "-"}</td>

      <td>{emailAddress ?? "-"}</td>

      <td>{formatAmount(totalAmount, currencyCode)}</td>

      <td>{currencyCode ?? "-"}</td>

      <td>{auditor ?? "-"}</td>

      <td>
        <span>{formatStatus(auditStatus)}</span>
      </td>

      <td>{formatDate(updatedAt)}</td>
    </tr>
  );
}