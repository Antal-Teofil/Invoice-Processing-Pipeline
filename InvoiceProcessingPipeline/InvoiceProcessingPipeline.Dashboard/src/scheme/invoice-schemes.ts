import { z } from "zod";

export const AUDIT_STATUSES = [
"extracted",
"failed",
"constraint_violation",
"under_review",
"rejected",
"approved",
"booked",
] as const;

/**
 * Crucial metadata about an document record
 * document
 */
export const DocumentMetadataScheme = z.object({
/**
 * Unique identifier of the document
 */
documentAuditId: z.guid(),
/**
 * Unique identifier of the orchestration workflow which processes the document
 */
workflowId: z.guid(),
/**
 * The workflow status of the document
 */
auditStatus: z.enum(AUDIT_STATUSES),
});

export const DocumentSummaryScheme = z.object({
  invoiceNumber: z.string().nullable().default(null),
  accountingSupplierParty: z.string().nullable().default(null),
  supplierPhoneNumber: z.e164().nullable().default(null),
  supplierEmailAddress: z.email().nullable().default(null),
  totalAmount: z.number().nullable().default(null),
  currencyCode: z.string().regex(/^[A-Z]{3}$/).nullable().default(null),
  updatedAt: z.iso.datetime({offset: true}),
  auditor: z.string().nullable().default(null),
});

export const DocumentMetadataDTOScheme = z.object({
...DocumentMetadataScheme.shape,
...DocumentSummaryScheme.shape,
});

export const DocumentSummaryRecordScheme = DocumentMetadataDTOScheme.omit({
workflowId: true,
});

export const DocumentMetadataDTOArrayScheme = z.array(DocumentMetadataDTOScheme);
