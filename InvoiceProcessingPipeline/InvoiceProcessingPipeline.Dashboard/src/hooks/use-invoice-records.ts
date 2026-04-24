import { useQuery } from "@tanstack/react-query";
import type { InvoiceSummaryRecord } from "../types/invoice-types";
import { fetchInvoiceSummaryRecordsAsync } from "../services/invoice-service";

export function useInvoiceSummaryRecords() {
  return useQuery<Array<InvoiceSummaryRecord>, Error>({
    queryKey: ["invoice-summary-records"],
    queryFn: fetchInvoiceSummaryRecordsAsync,
    retry: false,
    retryOnMount: false,
    refetchOnWindowFocus: false,
    refetchOnReconnect: false,
  });
}