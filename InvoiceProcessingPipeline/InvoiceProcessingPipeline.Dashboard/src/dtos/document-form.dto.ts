import z from "zod";
import AUDIT_STATUSES from "../shared/constants/audit-status.constant";
import { CommercialInvoiceFormSchema } from "../schemas/invoice/invoice.schema";

const InvoiceFormMetadataSchema = z.object({
  documentAuditId: z.guid(),
  workflowId: z.guid(),
  auditStatus: z.enum(AUDIT_STATUSES),
  updatedAt: z.iso.datetime({ offset: true }),
  documentResourceUri: z.url()
});

export const InvoiceFormSchema = z.object({
    header: InvoiceFormMetadataSchema,
    data: CommercialInvoiceFormSchema,
});

export type InvoiceFormType = z.infer<typeof InvoiceFormSchema>;