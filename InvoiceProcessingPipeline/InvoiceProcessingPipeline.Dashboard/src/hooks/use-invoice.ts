import { useQuery } from "@tanstack/react-query";
import type { Invoice } from "../types/invoice-types";
import { fetchInvoiceDataAsync } from "../services/invoice-service";


export function useInvoiceData( invoiceId : string ) {
    return useQuery<Invoice, Error>({
        queryKey: ["invoices", invoiceId],
        queryFn:() => fetchInvoiceDataAsync(invoiceId),
        enabled: !!invoiceId
    });
}