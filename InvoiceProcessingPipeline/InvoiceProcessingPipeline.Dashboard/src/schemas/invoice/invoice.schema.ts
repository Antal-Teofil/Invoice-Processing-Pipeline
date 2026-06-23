import z from "zod";

import { INVOICE_TYPE_CODES } from "../../shared/constants/invoice-type-code.constant";
import { CURRENCY_TYPE_CODES } from "../../shared/constants/currency-code.constant";

import AccountingPartyFormSchema from "./accounting-party.schema";
import AllowanceChargeFormSchema from "./allowance-charge.schema";
import TaxTotalFormSchema from "./tax-total.schema";
import LegalMonetaryTotalFormSchema from "./legal-monetary-total.schema";
import InvoiceLineFormSchema from "./invoice-line.schema";
import {
  arrayDefault,
  objectDefault,
} from "../../shared/utility/zod-default.utility";
import InvoicePeriodFormSchema from "./invoice-period.shema";

const CommercialInvoiceFormSchema = z.object({
  invoiceNumber: z.string().nullable().default(null),

  issueDate: z.iso.date().nullable().default(null),

  dueDate: z.iso.date().nullable().default(null),

  typeCode: z.enum(INVOICE_TYPE_CODES).nullable().default(null),

  note: z.string().nullable().default(null),

  taxPointDate: z.iso.date().nullable().default(null),

  documentCurrencyCode: z.enum(CURRENCY_TYPE_CODES).nullable().default(null),

  taxCurrencyCode: z.enum(CURRENCY_TYPE_CODES).nullable().default(null),

  invoicePeriod: objectDefault(InvoicePeriodFormSchema),

  accountingCustomerParty: objectDefault(AccountingPartyFormSchema),

  accountingSupplierParty: objectDefault(AccountingPartyFormSchema),

  allowanceCharge: arrayDefault(AllowanceChargeFormSchema),

  taxTotal: arrayDefault(TaxTotalFormSchema),

  legalMonetaryTotal: objectDefault(LegalMonetaryTotalFormSchema),

  invoiceLine: arrayDefault(InvoiceLineFormSchema),
});

export { CommercialInvoiceFormSchema };
