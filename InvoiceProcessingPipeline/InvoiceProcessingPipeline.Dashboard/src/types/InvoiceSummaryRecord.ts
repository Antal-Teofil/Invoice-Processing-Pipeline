
export type InvoiceSummaryRecord = {
    invoiceId: string;
    vendor?: string | null;
    phoneNumber?: string | null;
    vendorEmailAddress?: string | null;
    totalAmount?: number | null;
    currencyCode?: string | null;
    status: string;
};