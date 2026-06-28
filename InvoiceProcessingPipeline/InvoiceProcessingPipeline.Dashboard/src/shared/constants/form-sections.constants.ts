export const INVOICE_SECTION_IDS = {
  invoiceDetails: "invoice-details",
  parties: "parties",
  supplierParty: "supplier-party",
  customerParty: "customer-party",
  allowanceCharges: "allowance-charges",
  taxTotals: "tax-totals",
  legalMonetaryTotal: "legal-monetary-total",
  invoiceLines: "invoice-lines",

  allowanceChargeItem: (index: number) => `allowance-charge-${index}`,
  taxTotalItem: (index: number) => `tax-total-${index}`,
  invoiceLineItem: (index: number) => `invoice-line-${index}`,
} as const;