import z from "zod";
import { INVOICE_TYPE_CODES } from "../../shared/constants/invoice-type-code.constant";
import { CURRENCY_TYPE_CODES } from "../../shared/constants/currency-code.constant";
import InvoicePeriodFormSchema from "./invoice-period.shema";
import AccountingPartyFormSchema from "./accounting-party.schema";
import AllowanceChargeFormSchema from "./allowance-charge.schema";
import TaxTotalFormSchema from "./tax-total.schema";
import LegalMonetaryTotalFormSchema from "./legal-monetary-total.schema";
import InvoiceLineFormSchema from "./invoice-line.schema";

const CommercialInvoiceFormSchema = z.object({
    invoiceNumber: z.string().nullable().default(null),
    issueDate: z.iso.date().nullable().default(null),
    dueDate: z.iso.date().nullable().default(null),
    typeCode: z.enum(INVOICE_TYPE_CODES).nullable().default(null),
    note: z.string().nullable().default(null),
    taxPointDate: z.iso.date().nullable().default(null),
    documentCurrencyCode: z.enum(CURRENCY_TYPE_CODES).nullable().default(null),
    taxCurrencyCode: z.enum(CURRENCY_TYPE_CODES).nullable().default(null),
    invoicePeriod : InvoicePeriodFormSchema.nullable().default(null),
    accountingCustomerParty: AccountingPartyFormSchema.nullable().default(null),
    accountingSupplierParty: AccountingPartyFormSchema.nullable().default(null),
    allowanceCharge: z.array(AllowanceChargeFormSchema).nullable().default(null),
    taxTotal: z.array(TaxTotalFormSchema).nullable().default(null),
    legalMonetaryTotal: LegalMonetaryTotalFormSchema.nullable().default(null),
    invoiceLine: z.array(InvoiceLineFormSchema).nullable().default(null),
});

export {
    CommercialInvoiceFormSchema,
};