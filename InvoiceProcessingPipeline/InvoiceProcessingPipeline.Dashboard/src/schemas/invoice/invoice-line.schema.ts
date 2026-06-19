import z from "zod";
import AmountSchema from "./amount.schema";
import InvoicePeriodSchema from "./invoice-period.shema";
import AllowanceChargeSchema from "./allowance-charge.schema";
import { COUNTRY_CODES } from "../../shared/constants/country-code.constant";
import TaxCategorySchema from "./tax-category.schema";

const QuantitySchema = z.object({
    amount: z.number().nullable().default(null),
    unitCode: z.string().nullable().default(null)
});

const AdditionalItemPropertySchema = z.object({
    name: z.string().nullable().default(null),
    propertyValue: z.string().nullable().default(null)
}); 

const ItemSchema = z.object({
    description: z.string().nullable().default(null),
    name: z.string().nullable().default(null),
    countryOriginCode: z.enum(COUNTRY_CODES).nullable().default(null),
    classifiedTaxCategory: TaxCategorySchema.nullable().default(null),
    additionalItemProperty: z.array(AdditionalItemPropertySchema).nullable().default(null)
});

const PriceSchema = z.object({
    priceAmount: AmountSchema.nullable().default(null),
    baseQuantity: QuantitySchema.nullable().default(null),
    allowanceCharge: AllowanceChargeSchema.nullable().default(null)
});

const InvoiceLineSchema = z.object({
    lineId: z.string().nullable().default(null),
    note: z.string().nullable().default(null),
    invoicedQuantity: QuantitySchema.nullable().default(null),
    lineExtensionAmount: AmountSchema.nullable().default(null),
    invoicePeriod: InvoicePeriodSchema.nullable().default(null),
    allowanceCharge: z.array(AllowanceChargeSchema).nullable().default(null), 
    item: ItemSchema.nullable().default(null),
    price: PriceSchema.nullable().default(null)
});

export default InvoiceLineSchema;