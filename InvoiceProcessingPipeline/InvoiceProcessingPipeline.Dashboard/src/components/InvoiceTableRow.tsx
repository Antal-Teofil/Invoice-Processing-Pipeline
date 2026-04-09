export type InvoiceStatus =
  | "Approved"
  | "Correction Required"
  | "Cancelled"
  | "Under Review";

export type InvoiceRow = {
  invoiceId: string;
  vendorName: string;
  emailAddress: string;
  phoneNumber: string;
  amount: number;
  issueDate: string;
  status: InvoiceStatus;
};

const amountFormatter = new Intl.NumberFormat("en-US", {
  style: "currency",
  currency: "USD",
});

const dateFormatter = new Intl.DateTimeFormat("hu-HU", {
  year: "numeric",
  month: "2-digit",
  day: "2-digit",
});

function getStatusClasses(status: InvoiceStatus) {
  switch (status) {
    case "Approved":
      return "bg-emerald-50 text-emerald-700 ring-1 ring-emerald-200";
    case "Correction Required":
      return "bg-amber-50 text-amber-700 ring-1 ring-amber-200";
    case "Cancelled":
      return "bg-rose-50 text-rose-700 ring-1 ring-rose-200";
    case "Under Review":
      return "bg-sky-50 text-sky-700 ring-1 ring-sky-200";
    default:
      return "bg-slate-50 text-slate-700 ring-1 ring-slate-200";
  }
}

export default function InvoiceTableRow({
  invoiceId,
  vendorName,
  emailAddress,
  phoneNumber,
  amount,
  issueDate,
  status,
}: InvoiceRow) {
  return (
    <tr className="border-b border-slate-200/80 bg-white/70 transition-all duration-200 hover:bg-sky-50/70">
      <td className="px-4 py-4 align-middle">
        <span className="inline-flex whitespace-nowrap rounded-full border border-slate-200 bg-slate-900 px-3 py-1 text-xs font-semibold text-white shadow-sm">
          {invoiceId}
        </span>
      </td>

      <td className="px-4 py-4 align-middle">
        <div className="truncate text-sm font-semibold text-slate-900">
          {vendorName}
        </div>
      </td>

      <td className="px-4 py-4 align-middle">
        <a
          href={`mailto:${emailAddress}`}
          title={emailAddress}
          className="block truncate text-sm text-slate-600 transition-colors hover:text-sky-600"
        >
          {emailAddress}
        </a>
      </td>

      <td className="whitespace-nowrap px-4 py-4 align-middle text-sm text-slate-600 tabular-nums">
        {phoneNumber}
      </td>

      <td className="px-4 py-4 align-middle">
        <span className="inline-flex whitespace-nowrap rounded-full bg-emerald-50 px-3 py-1 text-[13px] font-semibold text-emerald-700 ring-1 ring-emerald-200">
          {amountFormatter.format(amount)}
        </span>
      </td>

      <td className="whitespace-nowrap px-4 py-4 align-middle text-sm text-slate-500 tabular-nums">
        {dateFormatter.format(new Date(issueDate))}
      </td>

      <td className="px-4 py-4 align-middle">
        <span
          className={`inline-flex whitespace-nowrap rounded-full px-3 py-1 text-[13px] font-semibold ${getStatusClasses(
            status
          )}`}
        >
          {status}
        </span>
      </td>
    </tr>
  );
}