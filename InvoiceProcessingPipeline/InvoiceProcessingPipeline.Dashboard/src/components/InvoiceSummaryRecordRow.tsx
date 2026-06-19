import { useNavigate } from "react-router-dom";
import type { InvoiceSummaryRecord } from "../types/invoice-summary-record";

function getStatusClasses(status: string) {
  switch (status) {
    case "extracted":
      return "bg-sky-50 text-sky-700 ring-sky-600/20";

    case "under_review":
      return "bg-amber-50 text-amber-700 ring-amber-600/20";

    case "constraint_violation":
      return "bg-orange-50 text-orange-700 ring-orange-600/20";

    case "approved":
      return "bg-emerald-50 text-emerald-700 ring-emerald-600/20";

    case "booked":
      return "bg-indigo-50 text-indigo-700 ring-indigo-600/20";

    case "rejected":
    case "failed":
      return "bg-rose-50 text-rose-700 ring-rose-600/20";

    default:
      return "bg-slate-50 text-slate-700 ring-slate-600/20";
  }
}

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
    navigate(`/invoices/${documentId}`);
  };

  return (
    <tr
      onClick={handleRowClick}
      className="cursor-pointer transition-colors odd:bg-white even:bg-slate-50/40 hover:bg-sky-50/40"
    >
      <td className="border-b border-slate-100 px-6 py-5 text-left text-sm font-semibold text-slate-900">
        {invoiceNumber ?? documentId}
      </td>

      <td className="border-b border-slate-100 px-6 py-5 text-left text-sm text-slate-700">
        {supplier ?? "-"}
      </td>

      <td className="border-b border-slate-100 px-6 py-5 text-center text-sm text-slate-600">
        {phoneNumber ?? "-"}
      </td>

      <td className="border-b border-slate-100 px-6 py-5 text-center text-sm text-slate-600">
        {emailAddress ?? "-"}
      </td>

      <td className="border-b border-slate-100 px-6 py-5 text-center text-sm font-medium text-slate-800">
        {formatAmount(totalAmount, currencyCode)}
      </td>

      <td className="border-b border-slate-100 px-6 py-5 text-center text-sm text-slate-600">
        {currencyCode ?? "-"}
      </td>

      <td className="border-b border-slate-100 px-6 py-5 text-center text-sm text-slate-600">
        {auditor ?? "-"}
      </td>

      <td className="border-b border-slate-100 px-6 py-5 text-center text-sm">
        <span
          className={`inline-flex min-w-[132px] items-center justify-center rounded-full px-3 py-1.5 text-xs font-semibold capitalize ring-1 ring-inset ${getStatusClasses(
            auditStatus
          )}`}
        >
          {formatStatus(auditStatus)}
        </span>
      </td>

      <td className="border-b border-slate-100 px-6 py-5 text-center text-sm text-slate-600">
        {formatDate(updatedAt)}
      </td>
    </tr>
  );
}