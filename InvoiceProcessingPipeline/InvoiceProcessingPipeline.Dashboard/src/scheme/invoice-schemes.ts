import { z } from 'zod';

export const AUDIT_STATUSES = [
  "extracted",
  "failed",
  "constraint_violation",
  "under_review",
  "rejected",
  "approved",
  "booked",
] as const;

const formatInvalidValue = (value: unknown): string =>
  typeof value === "number" && Number.isNaN(value)
    ? "NaN"
    : value === undefined
      ? "undefined"
      : JSON.stringify(value) ?? String(value);

const invalidInputFormatter =
  (fieldName: string, message: string) =>
  (issue: { input?: unknown }) =>
    `[${fieldName} (${formatInvalidValue(issue.input)})] ${message}`;

export const DocumentMetadataScheme = z.object({
  invoiceId: z.string({error: invalidInputFormatter('DOCUMENT ID', 'Invalid GUID!')}),
  processId: z.string({error: invalidInputFormatter('PROCESS ID', 'Invalid GUID!')}),
  auditStatus: z.enum(AUDIT_STATUSES, {error: invalidInputFormatter('STATUS', 'Invalid audit status!')})
});

export const DocumentSummaryScheme = z.object({
  vendorName: z.string({error: invalidInputFormatter('VENDOR NAME', 'Unrecognized input!')}).default('unknown'),
  phoneNumber: z.e164({error: invalidInputFormatter('PHONE NUMBER', 'Unrecognized input!')}).nullable().default('unknown'),
  vendorEmailAddress: z.email({error: invalidInputFormatter('VENDOR EMAIL ADDRESS', 'Unrecognized input!')}).nullable().default('unknown'),
  totalAmount: z.coerce.number({
    error: invalidInputFormatter('TOTAL AMOUNT', 'Unrecognized input')})
      .pipe(z.float32({
        error: invalidInputFormatter('TOTAL AMOUNT', 'Out of range value')})
      .min(0.0, {error: invalidInputFormatter('TOTAL AMOUNT', 'Negative input')}))
      .nullable()
      .default(0),
  currencyCode: z.string({error: invalidInputFormatter('CURRENCY CODE', 'Unrecognized input!')}).regex(/^[A-Z]{3}$/).nullable().default('unknown')
});

export const DocumentMetadataDTOScheme = z.object({
  ...DocumentMetadataScheme.shape,
  ...DocumentSummaryScheme.shape,
});

export const DocumentSummaryRecordScheme = DocumentMetadataDTOScheme.omit({processId: true});

export const DocumentMetadataDTOArrayScheme = z.array(DocumentMetadataDTOScheme);