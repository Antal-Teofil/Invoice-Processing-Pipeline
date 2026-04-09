import { apiClient } from "../clients/AxiosClient";
import type { InvoiceRow } from "../components/InvoiceTableRow";
import type { PaginatedResponse } from "../types/PaginatedResponse";

export type GetInvoicesRequest = {
  pageSize: number;
  continuationToken?: string | null;
};

export async function getInvoices({
  pageSize,
  continuationToken,
}: GetInvoicesRequest): Promise<PaginatedResponse<InvoiceRow>> {
  const response = await apiClient.get<PaginatedResponse<InvoiceRow>>("/invoices", {
    params: {
      pageSize,
      continuationToken: continuationToken ?? undefined,
    },
  });

  return response.data;
}