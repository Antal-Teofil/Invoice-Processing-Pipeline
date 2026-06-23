import z from "zod";
import AmountFormSchema from "./amount.schema";

const LegalMonetaryTotalFormSchema = z.object({
    lineExtensionAmount: AmountFormSchema.nullable().default(null),
    taxExclusiveAmount: AmountFormSchema.nullable().default(null),
    allowanceTotalAmount: AmountFormSchema.nullable().default(null),
    chargeTotalAmount: AmountFormSchema.nullable().default(null),
    prepaidAmount: AmountFormSchema.nullable().default(null),
    payableRoundingAmount: AmountFormSchema.nullable().default(null),
    payableAmount: AmountFormSchema.nullable().default(null),
});

export default LegalMonetaryTotalFormSchema;