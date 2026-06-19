import z from "zod";
import AmountSchema from "./amount.schema";
import TaxCategorySchema from "./tax-category.schema";

const TaxSubtotalSchema = z.object({
    taxableAmount: AmountSchema.nullable().default(null),
    taxAmount: AmountSchema.nullable().default(null),
    taxCategory: TaxCategorySchema.nullable().default(null),
});

export default TaxSubtotalSchema;