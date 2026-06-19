import type z from "zod";
import type { InvoiceMetadataSchema, InvoiceSummaryRecordDTOSchema, InvoiceSummaryRecordSchema } from "../dtos/document-summary-record.dto";
import type { PagedResult } from "../shared/utility/page-result.utility";


export type InvoiceMetadata = z.infer<typeof InvoiceMetadataSchema>;

export type InvoiceSummaryRecordDTO = z.infer<
  typeof InvoiceSummaryRecordDTOSchema
>;

export type InvoiceSummaryRecordData = z.infer<
  typeof InvoiceSummaryRecordSchema
>;

export type PagedInvoiceSummaryRecordDTO =
  PagedResult<InvoiceSummaryRecordDTO>;

export type InvoiceSummaryRecord = {
  documentId: string;
  auditStatus: string;
  updatedAt: string;
  invoiceNumber: string | null;
  supplier: string | null;
  phoneNumber: string | null;
  emailAddress: string | null;
  totalAmount: number | null;
  currencyCode: string | null;
  auditor: string | null;
};