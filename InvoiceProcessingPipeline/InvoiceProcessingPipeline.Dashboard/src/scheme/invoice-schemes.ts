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

export const DocumentMetadataScheme = z.object({
invoiceId: z.any(),
processId: z.any(),
auditStatus: z.any(),
});

export const DocumentSummaryScheme = z.object({
vendorName: z.any(),
phoneNumber: z.any(),
vendorEmailAddress: z.any(),
totalAmount: z.any(),
currencyCode: z.any(),
});

export const DocumentMetadataDTOScheme = z.object({
...DocumentMetadataScheme.shape,
...DocumentSummaryScheme.shape,
});

export const DocumentSummaryRecordScheme = DocumentMetadataDTOScheme.omit({
processId: true,
});

export const DocumentMetadataDTOArrayScheme = z.array(DocumentMetadataDTOScheme);
