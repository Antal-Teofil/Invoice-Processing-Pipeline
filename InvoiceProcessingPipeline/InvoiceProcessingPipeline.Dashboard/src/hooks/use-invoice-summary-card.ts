import type { InvoiceSummaryCard } from "../types/InvoiceSummaryCard";
import {create} from "zustand";


type InvoiceSummaryCardState = { items: Array<InvoiceSummaryCard> };

type InvoiceSummaryCardActions = {
    updateSummaryCardStatus: ()
};