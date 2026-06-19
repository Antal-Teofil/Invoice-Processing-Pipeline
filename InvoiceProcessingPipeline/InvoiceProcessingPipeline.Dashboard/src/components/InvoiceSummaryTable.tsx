import { useInvoiceSummaryRecords } from "../hooks/useInvoiceSummaryRecords.hook";
import { useInvoiceSummaryRecordTableStore } from "../stores/useInvoiceSummaryRecordTable.store";
import InvoiceSummaryRecordRow from "./InvoiceSummaryRecordRow";

function InvoiceSummaryTable() {
  const {
    records,
    pageSize,
    currentPageNumber,
    isLoading,
    isFetching,
    isFetchingNextPage,
    error,
    canGoToPreviousPage,
    canGoToNextPage,
    goToPreviousPage,
    goToNextPage,
  } = useInvoiceSummaryRecords();

  const setPageSize = useInvoiceSummaryRecordTableStore(
    (state) => state.setPageSize
  );

  if (isLoading) {
    return <div className="p-6 text-sm text-slate-600">Invoice records are loading...</div>;
  }

  if (error) {
    return <div className="p-6 text-sm text-rose-600">Failed to load invoice records.</div>;
  }

  return (
    <section className="overflow-hidden rounded-2xl border border-slate-200 bg-white shadow-sm">
      <div className="flex items-center justify-between border-b border-slate-200 px-6 py-4">
        <h2 className="text-base font-semibold text-slate-900">
          Invoice Records Overview
        </h2>

        {isFetching && !isFetchingNextPage && (
          <span className="text-sm text-slate-500">Refreshing...</span>
        )}
      </div>

      <div className="overflow-x-auto">
        <table className="min-w-full border-collapse">
          <caption className="sr-only">Invoice Records Overview</caption>

          <thead className="bg-slate-50">
            <tr>
              <th className="px-6 py-4 text-left text-xs font-semibold uppercase tracking-wide text-slate-500">
                ID
              </th>
              <th className="px-6 py-4 text-left text-xs font-semibold uppercase tracking-wide text-slate-500">
                Vendor Name
              </th>
              <th className="px-6 py-4 text-center text-xs font-semibold uppercase tracking-wide text-slate-500">
                Phone Number
              </th>
              <th className="px-6 py-4 text-center text-xs font-semibold uppercase tracking-wide text-slate-500">
                E-mail Address
              </th>
              <th className="px-6 py-4 text-center text-xs font-semibold uppercase tracking-wide text-slate-500">
                Total Amount
              </th>
              <th className="px-6 py-4 text-center text-xs font-semibold uppercase tracking-wide text-slate-500">
                Currency
              </th>
              <th className="px-6 py-4 text-center text-xs font-semibold uppercase tracking-wide text-slate-500">
                Auditor
              </th>
              <th className="px-6 py-4 text-center text-xs font-semibold uppercase tracking-wide text-slate-500">
                Status
              </th>
              <th className="px-6 py-4 text-center text-xs font-semibold uppercase tracking-wide text-slate-500">
                Last Seen
              </th>
            </tr>
          </thead>

          <tbody>
            {records.length > 0 ? (
              records.map((record) => (
                <InvoiceSummaryRecordRow
                  key={record.documentId}
                  {...record}
                />
              ))
            ) : (
              <tr>
                <td
                  colSpan={9}
                  className="px-6 py-10 text-center text-sm text-slate-500"
                >
                  No invoice records found.
                </td>
              </tr>
            )}
          </tbody>
        </table>
      </div>

      <footer className="flex items-center justify-between border-t border-slate-200 px-6 py-4">
        <button
          type="button"
          onClick={goToPreviousPage}
          disabled={!canGoToPreviousPage || isFetchingNextPage}
          className="rounded-lg border border-slate-300 px-4 py-2 text-sm font-medium text-slate-700 disabled:cursor-not-allowed disabled:opacity-50"
        >
          Previous
        </button>

        <div className="flex items-center gap-4">
          <span className="text-sm text-slate-600">
            Page {currentPageNumber}
          </span>

          <select
            value={pageSize}
            onChange={(event) => setPageSize(Number(event.target.value))}
            disabled={isFetchingNextPage}
            className="rounded-lg border border-slate-300 px-3 py-2 text-sm text-slate-700 disabled:cursor-not-allowed disabled:opacity-50"
          >
            <option value={10}>10 / page</option>
            <option value={20}>20 / page</option>
            <option value={50}>50 / page</option>
            <option value={100}>100 / page</option>
          </select>
        </div>

        <button
          type="button"
          onClick={goToNextPage}
          disabled={!canGoToNextPage || isFetchingNextPage}
          className="rounded-lg border border-slate-300 px-4 py-2 text-sm font-medium text-slate-700 disabled:cursor-not-allowed disabled:opacity-50"
        >
          {isFetchingNextPage ? "Loading..." : "Next"}
        </button>
      </footer>
    </section>
  );
}

export default InvoiceSummaryTable;