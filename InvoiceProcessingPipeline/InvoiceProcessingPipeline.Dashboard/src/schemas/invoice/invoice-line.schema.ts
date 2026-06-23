import z from "zod";
import AmountSchema from "./amount.schema";
import InvoicePeriodSchema from "./invoice-period.shema";
import AllowanceChargeSchema from "./allowance-charge.schema";
import { QuantityFormSchema } from "./quantity.schema";
import { ItemFormSchema } from "./item.schema";
import { PriceFormSchema } from "./price.schema";

const InvoiceLineFormSchema = z.object({
    lineId: z.string().nullable().default(null),
    note: z.string().nullable().default(null),
    invoicedQuantity: QuantityFormSchema.nullable().default(null),
    lineExtensionAmount: AmountSchema.nullable().default(null),
    invoicePeriod: InvoicePeriodSchema.nullable().default(null),
    allowanceCharge: z.array(AllowanceChargeSchema).nullable().default(null), 
    item: ItemFormSchema.nullable().default(null),
    price: PriceFormSchema.nullable().default(null)
});

export default InvoiceLineFormSchema;