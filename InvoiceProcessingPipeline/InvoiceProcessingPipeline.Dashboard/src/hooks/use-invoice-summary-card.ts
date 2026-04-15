import type { InvoiceSummaryCard } from "../types/InvoiceSummaryCard";
import {create} from "zustand";

type InvoiceSummaryCardCollection = {};

export const useInvoiceSummaryCardCollection = create<InvoiceSummaryCardCollection>()((set) => ({
    invoiceSummaryItems: Array<InvoiceSummaryCard>,
    updateInvoiceStatus: (id: string, status: string) => void
}));