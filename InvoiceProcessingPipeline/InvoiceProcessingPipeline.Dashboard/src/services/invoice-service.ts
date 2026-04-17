import { axiosClient } from "../clients/AxiosClient";
import { DocumentRecordMetadataSchema, type DocumentRecordMetadata } from "../dtos/DocumentRecordMetadata";
import type { InvoiceSummaryRecord } from "../types/InvoiceSummaryRecord";
import { z } from "zod";



export async function fetchInvoiceRecords(): Promise<InvoiceSummaryRecord[]> {
  const { data } = await axiosClient.get<unknown>("/api/audit/records");

  const DocumentRecordMetadataArraySchema = z.array(DocumentRecordMetadataSchema);
  const result = await DocumentRecordMetadataArraySchema.safeParseAsync(data);

  if (!result.success) {
    console.error(result.error.issues);
    throw new Error("Nem sikerült parsolni a DocumentRecordMetadata objektumokat.");
  }

  const records: DocumentRecordMetadata[] = result.data;

  return records.map((record) => convertToInvoiceSummaryCard(record));
}

export function convertToInvoiceSummaryCard(
  record: DocumentRecordMetadata
): InvoiceSummaryRecord {
  return {
    invoiceId: record.invoiceId,
    vendor: record.vendorName,
    phoneNumber: record.phoneNumber,
    vendorEmailAddress: record.vendorEmailAddress,
    totalAmount: record.totalAmount,
    currencyCode: record.currencyCode,
    status: record.auditStatus,
  };
}