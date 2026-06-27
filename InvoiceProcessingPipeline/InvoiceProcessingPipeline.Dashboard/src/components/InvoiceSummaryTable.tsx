import { useInvoiceSummaryRecords } from "../hooks/useInvoiceSummaryRecords.hook";
import { useInvoiceSummaryRecordTableStore } from "../stores/record-table.store";
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
    return (
      <div className="summary-table-state">
        Invoice records are loading...
      </div>
    );
  }

  if (error) {
    return (
      <div className="summary-table-state summary-table-error">
        Failed to load invoice records.
      </div>
    );
  }

  return (
    <section className="summary-table-section">
      <div className="summary-table-card">
        <header className="summary-table-header">
          <h2 className="summary-table-title">
            Invoice Records Overview
          </h2>

          {isFetching && !isFetchingNextPage && (
            <span className="summary-table-refreshing">
              Refreshing...
            </span>
          )}
        </header>

        <div className="summary-table-wrapper">
          <table className="summary-table">
            <thead className="summary-table-head">
              <tr>
                <th className="summary-table-th">ID</th>
                <th className="summary-table-th">Vendor Name</th>
                <th className="summary-table-th">Phone Number</th>
                <th className="summary-table-th">E-mail Address</th>
                <th className="summary-table-th">Total Amount</th>
                <th className="summary-table-th">Currency</th>
                <th className="summary-table-th">Auditor</th>
                <th className="summary-table-th">Status</th>
                <th className="summary-table-th">Last Seen</th>
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
                  <td className="summary-table-empty-cell" colSpan={9}>
                    No invoice records found.
                  </td>
                </tr>
              )}
            </tbody>
          </table>
        </div>

        <footer className="summary-table-footer">
          <button
            type="button"
            className="summary-table-button"
            onClick={goToPreviousPage}
            disabled={!canGoToPreviousPage || isFetchingNextPage}
          >
            Previous
          </button>

          <div className="summary-table-pagination">
            <span className="summary-table-page-label">
              Page {currentPageNumber}
            </span>

            <select
              className="summary-table-select"
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
            className="summary-table-button summary-table-button-primary"
            onClick={goToNextPage}
            disabled={!canGoToNextPage || isFetchingNextPage}
          >
            {isFetchingNextPage ? "Loading..." : "Next"}
          </button>
        </footer>
      </div>
    </section>
  );
}

export default InvoiceSummaryTable;