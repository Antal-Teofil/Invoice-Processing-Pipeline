const AUDIT_STATUSES = [
  "extracted",
  "failed",
  "constraint_violation",
  "under_review",
  "rejected",
  "approved",
  "booked",
] as const;

export default AUDIT_STATUSES;