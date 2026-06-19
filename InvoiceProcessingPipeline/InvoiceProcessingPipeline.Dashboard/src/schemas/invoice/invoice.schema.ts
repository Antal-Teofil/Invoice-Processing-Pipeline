import z from "zod";
import { INVOICE_TYPE_CODES } from "../../shared/constants/invoice-type-code.constant";
import { CURRENCY_TYPE_CODES } from "../../shared/constants/currency-code.constant";
import InvoicePeriodSchema from "./invoice-period.shema";
import AccountingPartySchema from "./accounting-party.schema";
import AllowanceChargeSchema from "./allowance-charge.schema";
import TaxTotalSchema from "./tax-total.schema";
import LegalMonetaryTotalSchema from "./legal-monetary-total.schema";
import InvoiceLineSchema from "./invoice-line.schema";

const CommercialInvoiceFormSchema = z.object({
    invoiceNumber: z.string().nullable().default(null),
    issueDate: z.iso.date().nullable().default(null),
    dueDate: z.iso.date().nullable().default(null),
    typeCode: z.enum(INVOICE_TYPE_CODES).nullable().default(null),
    note: z.string().nullable().default(null),
    taxPointDate: z.iso.date().nullable().default(null),
    documentCurrencyCode: z.enum(CURRENCY_TYPE_CODES).nullable().default(null),
    taxCurrencyCode: z.enum(CURRENCY_TYPE_CODES).nullable().default(null),
    invoicePeriod : InvoicePeriodSchema.nullable().default(null),
    accountingCustomerParty: AccountingPartySchema.nullable().default(null),
    accountingSupplierParty: AccountingPartySchema.nullable().default(null),
    allowanceCharge: z.array(AllowanceChargeSchema).nullable().default(null),
    taxTotal: z.array(TaxTotalSchema).nullable().default(null),
    legalMonetaryTotal: LegalMonetaryTotalSchema.nullable().default(null),
    invoiceLine: z.array(InvoiceLineSchema).nullable().default(null),
});

export {
    CommercialInvoiceFormSchema,
};