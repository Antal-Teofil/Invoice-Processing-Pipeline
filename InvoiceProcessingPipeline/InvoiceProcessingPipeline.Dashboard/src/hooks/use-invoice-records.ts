import { useQuery } from "@tanstack/react-query";
import type { InvoiceSummaryRecord } from "../types/InvoiceSummaryRecord";
import { fetchInvoiceRecords } from "../services/invoice-service";


export function useInvoiceSummaryRecords() {
    return useQuery<Array<InvoiceSummaryRecord>, Error>({
        queryKey: ["invoice-summary-records"],
        queryFn: fetchInvoiceRecords,
    });
}