import { useMemo } from "react";

import type { IssueType } from "../types/issue.type";

export function useIssueSummary(issues: IssueType[]) {
  return useMemo(() => {
    const errorCount = issues.filter(
      (issue) => issue.severity === "fatal"
    ).length;

    const warningCount = issues.filter(
      (issue) => issue.severity === "warning"
    ).length;

    return {
      errorCount,
      warningCount,
      totalCount: errorCount + warningCount,
    };
  }, [issues]);
}