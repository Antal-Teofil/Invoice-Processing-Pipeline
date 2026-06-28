import type { IssueType } from "../types/issue.type";

type IssueCardProps = {
  issue: IssueType;
  onSelect: (sectionName: string | null) => void;
};

function formatConfidence(confidence: number | null | undefined) {
  if (confidence === null || confidence === undefined) {
    return null;
  }

  const normalizedConfidence = Math.min(Math.max(confidence, 0), 1);

  return `${Math.round(normalizedConfidence * 100)}%`;
}

export function IssueCard({ issue, onSelect }: IssueCardProps) {
  const {
    severity,
    issueCode,
    category,
    description,
    section,
    confidence,
  } = issue;

  const confidenceLabel = formatConfidence(confidence);

  return (
    <button
      type="button"
      className="issue-card"
      data-severity={severity ?? undefined}
      onClick={() => onSelect(section)}
    >
      <div>
        {severity && <span>{severity}</span>}
        {category && <small>{category}</small>}
      </div>

      <strong>{issueCode ?? "UNKNOWN_ISSUE"}</strong>

      {description && <p>{description}</p>}

      {(section || confidenceLabel) && (
        <div>
          {section && <small>{section}</small>}

          {confidenceLabel && <span>Confidence: {confidenceLabel}</span>}
        </div>
      )}
    </button>
  );
}