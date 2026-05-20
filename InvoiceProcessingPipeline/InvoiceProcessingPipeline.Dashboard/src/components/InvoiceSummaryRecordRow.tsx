import { useNavigate } from "react-router-dom";
import type { InvoiceSummaryRecord } from "../types/invoice-types";

function getStatusClasses(status: string) {
  switch (status.toLowerCase()) {
    case "approved":
      return "bg-emerald-50 text-emerald-700 ring-emerald-200";
    case "under_review":
      return "bg-amber-50 text-amber-700 ring-amber-200";
    case "constraint_violation":
    case "failed":
    case "rejected":
      return "bg-red-50 text-red-700 ring-red-200";
    case "booked":
      return "bg-blue-50 text-blue-700 ring-blue-200";
    case "extracted":
      return "bg-sky-50 text-sky-700 ring-sky-200";
    default:
      return "bg-slate-100 text-slate-700 ring-slate-200";
  }
}

export default function InvoiceSummaryRecordRow({
  documentId,
  vendorName,
  vendorEmailAddress,
  vendorPhoneNumber,
  totalAmount,
  currencyCode,
  auditStatus,
}: InvoiceSummaryRecord) {

  const navigate = useNavigate();

  const handleRowClick = () => {
    navigate(`/invoices/${documentId}`);
  };

  return (
    <tr 
    onClick={handleRowClick}
    className="transition-colors odd:bg-white even:bg-slate-50/40 hover:bg-sky-50/40">
      <td className="border-b border-slate-100 px-6 py-5 text-left text-sm font-semibold text-slate-900">
        {documentId}
      </td>

      <td className="border-b border-slate-100 px-6 py-5 text-left text-sm text-slate-700">
        {vendorName ?? "-"}
      </td>

      <td className="border-b border-slate-100 px-6 py-5 text-center text-sm text-slate-600">
        {vendorEmailAddress ?? "-"}
      </td>

      <td className="border-b border-slate-100 px-6 py-5 text-center text-sm text-slate-600">
        {vendorPhoneNumber ?? "-"}
      </td>

      <td className="border-b border-slate-100 px-6 py-5 text-center text-sm font-medium text-slate-800">
        {typeof totalAmount === "number" ? totalAmount.toLocaleString() : "-"}
      </td>

      <td className="border-b border-slate-100 px-6 py-5 text-center text-sm text-slate-600">
        {currencyCode ?? "-"}
      </td>

      <td className="border-b border-slate-100 px-6 py-5 text-center text-sm">
        <span
          className={`inline-flex min-w-[92px] items-center justify-center rounded-full px-3 py-1.5 text-xs font-semibold ring-1 ring-inset ${getStatusClasses(
            auditStatus
          )}`}
        >
          {auditStatus}
        </span>
      </td>
    </tr>
  );
}