import z from "zod";
import AUDIT_STATUSES from "../shared/constants/audit-status.constant";
import { CommercialInvoiceFormSchema } from "../schemas/invoice/invoice.schema";
import { IssueShema } from "../schemas/issue.schema";

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
    issues: z.array(IssueShema)
});

export type InvoiceFormType = z.infer<typeof InvoiceFormSchema>;