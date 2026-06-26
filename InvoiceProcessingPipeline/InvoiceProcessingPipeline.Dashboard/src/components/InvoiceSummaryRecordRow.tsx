import { Link } from "react-router-dom";
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
  return (
    <tr className="summary-card-row">
      <td className="summary-card-cell summary-card-cell-strong">
        <Link
          to={`/editor/${documentId}`}
          aria-label={`Edit invoice ${invoiceNumber ?? documentId}`}
          className="summary-card-link"
        >
          {invoiceNumber ?? "-"}
        </Link>
      </td>

      <td className="summary-card-cell">{supplier ?? "-"}</td>

      <td className="summary-card-cell summary-card-cell-muted">
        {phoneNumber ?? "-"}
      </td>

      <td className="summary-card-cell summary-card-cell-muted">
        {emailAddress ?? "-"}
      </td>

      <td className="summary-card-cell summary-card-cell-strong">
        {formatAmount(totalAmount, currencyCode)}
      </td>

      <td className="summary-card-cell">{currencyCode ?? "-"}</td>

      <td className="summary-card-cell">{auditor ?? "-"}</td>

      <td className="summary-card-cell">
        <span className="summary-card-status" data-status={auditStatus}>
          {formatStatus(auditStatus)}
        </span>
      </td>

      <td className="summary-card-cell summary-card-cell-muted">
        {formatDate(updatedAt)}
      </td>
    </tr>
  );
}
