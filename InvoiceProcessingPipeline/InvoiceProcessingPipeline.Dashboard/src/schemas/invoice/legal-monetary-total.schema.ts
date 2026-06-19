import z from "zod";
import AmountSchema from "./amount.schema";

const LegalMonetaryTotalSchema = z.object({
    lineExtensionAmount: AmountSchema.nullable().default(null),
    taxExclusiveAmount: AmountSchema.nullable().default(null),
    allowanceTotalAmount: AmountSchema.nullable().default(null),
    chargeTotalAmount: AmountSchema.nullable().default(null),
    prepaidAmount: AmountSchema.nullable().default(null),
    payableRoundingAmount: AmountSchema.nullable().default(null),
    payableAmount: AmountSchema.nullable().default(null),
});

export default LegalMonetaryTotalSchema;