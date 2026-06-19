import z from "zod";
import AmountSchema from "./amount.schema";
import TaxCategorySchema from "./tax-category.schema";

const AllowanceChargeSchema = z.object({
    chargeIndicator: z.boolean().nullable().default(null),
    allowanceChargeReasonCode: z.string().nullable().default(null),
    allowanceChargeReason: z.string().nullable().default(null),
    multiplierFactorNumeric: z.number().nullable().default(null),
    amount: AmountSchema.nullable().default(null),
    baseAmount: AmountSchema.nullable().default(null),
    taxCategory: TaxCategorySchema.nullable().default(null),
});

export default AllowanceChargeSchema;