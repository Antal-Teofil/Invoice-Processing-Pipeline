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
    return <div>Invoice records are loading...</div>;
  }

  if (error) {
    return <div>Failed to load invoice records.</div>;
  }

  return (
    <section>
      <header>
        <h2>Invoice Records Overview</h2>

        {isFetching && !isFetchingNextPage && <span>Refreshing...</span>}
      </header>

      <table>
        <thead>
          <tr>
            <th>ID</th>
            <th>Vendor Name</th>
            <th>Phone Number</th>
            <th>E-mail Address</th>
            <th>Total Amount</th>
            <th>Currency</th>
            <th>Auditor</th>
            <th>Status</th>
            <th>Last Seen</th>
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
              <td colSpan={9}>No invoice records found.</td>
            </tr>
          )}
        </tbody>
      </table>

      <footer>
        <button
          type="button"
          onClick={goToPreviousPage}
          disabled={!canGoToPreviousPage || isFetchingNextPage}
        >
          Previous
        </button>

        <div>
          <span>Page {currentPageNumber}</span>

          <select
            value={pageSize}
            onChange={(event) => setPageSize(Number(event.target.value))}
            disabled={isFetchingNextPage}
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
        >
          {isFetchingNextPage ? "Loading..." : "Next"}
        </button>
      </footer>
    </section>
  );
}

export default InvoiceSummaryTable;