import type { Issue } from "../types/issue";

export function AlertCard(issue: Issue) {
  const colors: Record<Issue["severity"], string> = {
    error: "bg-red-100 border-red-500 text-red-800",
    warning: "bg-yellow-100 border-yellow-500 text-yellow-800",
    info: "bg-green-100 border-green-500 text-green-800",
  };

  return (
    <div className={`border-l-4 rounded-md p-4 mb-3 ${colors[issue.severity]}`}>
      <div className="font-bold uppercase">
        {issue.severity}
      </div>

      <div className="mt-1">
        <strong>Rule:</strong> {issue.rule}
      </div>

      {issue.message && (
        <div className="mt-1">
          {issue.message}
        </div>
      )}

      <div className="mt-1 text-sm">
        <strong>Field:</strong> {issue.fieldName}
      </div>
    </div>
  );
}