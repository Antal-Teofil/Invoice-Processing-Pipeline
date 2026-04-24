import { useInvoiceSummaryRecords } from "../hooks/use-invoice-records";
import InvoiceSummaryRecordRow from "./InvoiceSummaryRecordRow";

export default function InvoiceSummaryTable() {
  const { data, isPending, isError, error } = useInvoiceSummaryRecords();

  if (isPending) {
    return (
      <div className="rounded-3xl border border-slate-200 bg-white p-10 shadow-sm">
        <div className="text-center text-sm font-medium text-slate-500">
          Loading invoices...
        </div>
      </div>
    );
  }

  if (isError) {
    return (
      <div className="rounded-3xl border border-red-200 bg-red-50 p-8 shadow-sm">
        <p className="text-sm font-semibold text-red-700">Error</p>
        <p className="mt-1 text-sm text-red-600">{error.message}</p>
      </div>
    );
  }

  const records = data ?? [];

  return (
    <section className="mx-auto w-full max-w-7xl px-4 py-8 sm:px-6 lg:px-8">
      <div className="overflow-hidden rounded-3xl border border-slate-200 bg-white shadow-lg shadow-slate-200/50">
        <div className="flex flex-col gap-4 border-b border-slate-200 bg-gradient-to-r from-white to-slate-50 px-6 py-5 sm:flex-row sm:items-center sm:justify-between">
          <div>
            <h2 className="text-2xl font-semibold tracking-tight text-slate-900">
              Invoice List
            </h2>
            <p className="mt-1 text-sm text-slate-500">
              Overview of extracted invoice records
            </p>
          </div>

          <div className="inline-flex w-fit items-center rounded-full bg-slate-100 px-4 py-2 text-sm font-semibold text-slate-700 ring-1 ring-slate-200">
            {records.length} item{records.length === 1 ? "" : "s"}
          </div>
        </div>

        <div className="overflow-x-auto">
          <table className="min-w-full border-separate border-spacing-0">
            <caption className="sr-only">Invoice list</caption>

            <thead>
              <tr className="bg-slate-50">
                <th className="border-b border-slate-200 px-6 py-4 text-center text-xs font-bold uppercase tracking-[0.12em] text-slate-500">
                  ID
                </th>
                <th className="border-b border-slate-200 px-6 py-4 text-center text-xs font-bold uppercase tracking-[0.12em] text-slate-500">
                  Vendor Name
                </th>
                <th className="border-b border-slate-200 px-6 py-4 text-center text-xs font-bold uppercase tracking-[0.12em] text-slate-500">
                  E-mail Address
                </th>
                <th className="border-b border-slate-200 px-6 py-4 text-center text-xs font-bold uppercase tracking-[0.12em] text-slate-500">
                  Phone Number
                </th>
                <th className="border-b border-slate-200 px-6 py-4 text-center text-xs font-bold uppercase tracking-[0.12em] text-slate-500">
                  Total
                </th>
                <th className="border-b border-slate-200 px-6 py-4 text-center text-xs font-bold uppercase tracking-[0.12em] text-slate-500">
                  Currency
                </th>
                <th className="border-b border-slate-200 px-6 py-4 text-center text-xs font-bold uppercase tracking-[0.12em] text-slate-500">
                  Status
                </th>
              </tr>
            </thead>

            <tbody className="bg-white">
              {records.length === 0 ? (
                <tr>
                  <td
                    colSpan={7}
                    className="px-6 py-12 text-center text-sm font-medium text-slate-500"
                  >
                    No invoices found.
                  </td>
                </tr>
              ) : (
                records.map((record) => (
                  <InvoiceSummaryRecordRow
                    key={record.documentId}
                    {...record}
                  />
                ))
              )}
            </tbody>
          </table>
        </div>
      </div>
    </section>
  );
}