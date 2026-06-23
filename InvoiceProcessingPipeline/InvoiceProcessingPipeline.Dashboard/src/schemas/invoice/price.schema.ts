import z from "zod";
import { QuantityFormSchema } from "./quantity.schema";
import AllowanceChargeFormSchema from "./allowance-charge.schema";
import AmountFormSchema from "./amount.schema";

export const PriceFormSchema = z.object({
    priceAmount: AmountFormSchema.nullable().default(null),
    baseQuantity: QuantityFormSchema.nullable().default(null),
    allowanceCharge: AllowanceChargeFormSchema.nullable().default(null)
});