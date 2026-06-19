import type { InvoiceSummaryRecord, InvoiceSummaryRecordDTO } from "../types/invoice-summary-record";

export function mapInvoiceSummaryRecordDtoToTableRow(
  dto: InvoiceSummaryRecordDTO
): InvoiceSummaryRecord {
  return {
    documentId: dto.header.documentAuditId,
    auditStatus: dto.header.auditStatus,
    updatedAt: dto.header.updatedAt,

    invoiceNumber: dto.data.invoiceNumber,
    supplier: dto.data.accountingSupplierParty,
    phoneNumber: dto.data.supplierPhoneNumber,
    emailAddress: dto.data.supplierEmailAddress,
    totalAmount: dto.data.totalAmount,
    currencyCode: dto.data.currencyCode,
    auditor: dto.data.auditor,
  };
}