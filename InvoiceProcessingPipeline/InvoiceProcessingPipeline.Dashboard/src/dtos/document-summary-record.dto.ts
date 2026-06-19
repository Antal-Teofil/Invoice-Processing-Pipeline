import z from "zod";
import AUDIT_STATUSES from "../shared/constants/audit-status.constant";

const InvoiceMetadataSchema = z.object({
  documentAuditId: z.guid(),
  workflowId: z.guid(),
  auditStatus: z.enum(AUDIT_STATUSES),
  updatedAt: z.iso.datetime({ offset: true }),
});

const InvoiceSummaryRecordSchema = z.object({
  invoiceNumber: z.string().nullable().default(null),
  accountingSupplierParty: z.string().nullable().default(null),
  supplierPhoneNumber: z.e164().nullable().default(null),
  supplierEmailAddress: z.email().nullable().default(null),
  totalAmount: z.number().nullable().default(null),
  currencyCode: z
    .string()
    .regex(/^[A-Z]{3}$/)
    .nullable()
    .default(null),
  auditor: z.string().nullable().default(null),
});

const InvoiceSummaryRecordDTOSchema = z.object({
  header: InvoiceMetadataSchema,
  data: InvoiceSummaryRecordSchema,
});

const InvoiceSummaryRecordDTOCollectionSchema = z.array(
  InvoiceSummaryRecordDTOSchema
);

const PagedInvoiceSummaryRecordDTOSchema = z.object({
  items: InvoiceSummaryRecordDTOCollectionSchema,
  continuationToken: z.string().nullable(),
  isAny: z.boolean(),
});

export {
  InvoiceMetadataSchema,
  InvoiceSummaryRecordSchema,
  InvoiceSummaryRecordDTOSchema,
  InvoiceSummaryRecordDTOCollectionSchema,
  PagedInvoiceSummaryRecordDTOSchema,
};