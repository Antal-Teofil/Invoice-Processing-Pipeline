import type { IssueType } from "../types/issue.type";
import { IssueSummaryCard } from "./IssueSummaryCard";
import { IssueDrawer } from "./IssueDrawer";
import { useIssueSummary } from "../hooks/issue-summary.hook";

type IssueBoardProps = {
  issues: IssueType[];
};

export function IssueBoard({ issues }: IssueBoardProps) {
  const { errorCount, warningCount, totalCount } = useIssueSummary(issues);

  return (
    <>
      <IssueSummaryCard
        errorCount={errorCount}
        warningCount={warningCount}
        totalCount={totalCount}
      />

      <IssueDrawer issues={issues} />
    </>
  );
}