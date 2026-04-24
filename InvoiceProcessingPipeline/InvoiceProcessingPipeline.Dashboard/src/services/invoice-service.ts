import { axiosClient } from "../clients/AxiosClient";
import { DocumentMetadataDTOArrayScheme } from "../scheme/invoice-schemes";
import type { Invoice, InvoiceSummaryRecord } from "../types/invoice-types";

export async function fetchInvoiceSummaryRecordsAsync(): Promise<InvoiceSummaryRecord[]> {
  const { data } = await axiosClient.get<unknown>("/audit/records");

  const parsedData = await DocumentMetadataDTOArrayScheme.safeParseAsync(data);

  if (!parsedData.success) {
    console.log(
      "Error at parsing record data:",
      parsedData.error.issues.map((issue) => issue.message)
    );

    return [];
  }

  return parsedData.data.map(({ processId: _processId, ...summary }) => summary);
}

export async function fetchInvoiceDataAsync( invoiceId: string ) : Promise<Invoice> {

  const { data } = await axiosClient.get<unknown>(`verify/${invoiceId}`);

  return data as Invoice;
};