import { useIssueStore } from "../stores/issue.store";

type IssueSummaryCardProps = {
  errorCount: number;
  warningCount: number;
  totalCount: number;
};

export function IssueSummaryCard({
  errorCount,
  warningCount,
  totalCount,
}: IssueSummaryCardProps) {
  const openDrawer = useIssueStore((state) => state.openDrawer);

  const hasIssues = totalCount > 0;

  return (
    <button
      type="button"
      className="issue-summary-card"
      onClick={openDrawer}
      disabled={!hasIssues}
      aria-label="Open invoice issues"
    >
      <div data-issue-summary-item="error">
        <span>Errors</span>
        <strong>{errorCount}</strong>
      </div>

      <div data-issue-summary-item="warning">
        <span>Warnings</span>
        <strong>{warningCount}</strong>
      </div>

      <div data-issue-summary-item="total">
        <span>Total</span>
        <strong>{totalCount}</strong>
      </div>
    </button>
  );
}