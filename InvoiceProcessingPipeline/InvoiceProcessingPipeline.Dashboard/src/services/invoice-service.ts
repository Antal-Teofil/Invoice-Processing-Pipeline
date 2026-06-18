import { axiosClient } from "../clients/AxiosClient";
import { DocumentMetadataDTOArrayScheme } from "../scheme/invoice-schemes";
import type { Invoice, InvoiceSummaryRecord } from "../types/invoice-types";

export async function fetchInvoiceSummaryRecordsAsync(): Promise<Array<InvoiceSummaryRecord>> {
  const { data } = await axiosClient.get<unknown>("/audit/records");

  const parsedData = await DocumentMetadataDTOArrayScheme.safeParseAsync(data);

}

export async function fetchInvoiceDataAsync( invoiceId: string ) : Promise<Invoice> {

  const { data } = await axiosClient.get<unknown>(`verify/${invoiceId}`);

  return data as Invoice;
};