import { axiosClient } from "../clients/AxiosClient";
import { PagedInvoiceSummaryRecordDTOSchema } from "../dtos/document-summary-record.dto";
import type { PagedInvoiceSummaryRecordDTO } from "../types/invoice-summary-record";

type FetchInvoiceSummaryRecordsParams = {
  pageSize?: number;
  continuationToken?: string | null;
};

export async function fetchInvoiceSummaryRecordsAsync({
  pageSize = 20,
  continuationToken = null,
}: FetchInvoiceSummaryRecordsParams = {}): Promise<PagedInvoiceSummaryRecordDTO> {
  const { data } = await axiosClient.get<unknown>("/audit/records", {
    params: {
      pageSize,
      continuationToken: continuationToken ?? undefined,
    },
  });

  return PagedInvoiceSummaryRecordDTOSchema.parse(data);
}