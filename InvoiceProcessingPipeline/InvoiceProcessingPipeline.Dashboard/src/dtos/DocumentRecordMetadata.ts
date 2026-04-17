import * as z from "zod";

const AUDIT_STATUSES = [
  "extracted",
  "failed",
  "constraint_violation",
  "under_review",
  "rejected",
  "approved",
  "booked",
] as const;

export const DocumentRecordMetadataSchema = z.object({
  auditStatus: z.enum(AUDIT_STATUSES, {
    error: (issue) => `Unknown audit status: ${JSON.stringify(issue.input)}`,
  }),
  processId: z.guid({
    error: (issue) => `Invalid process identifier format: ${JSON.stringify(issue.input)}`,
  }),
  invoiceId: z.string(),
  vendorName: z.string().nullable(),
  phoneNumber: z.string().nullable(),
  vendorEmailAddress: z.email().nullable(),
  totalAmount: z.number().nonnegative().nullable(),
  currencyCode: z.string().nullable(),
});

export type DocumentRecordMetadata = z.infer<typeof DocumentRecordMetadataSchema>;