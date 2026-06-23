import z from "zod";
import AmountSchema from "./amount.schema";
import TaxCategorySchema from "./tax-category.schema";

const TaxSubtotalFormSchema = z.object({
    taxableAmount: AmountSchema.nullable().default(null),
    taxAmount: AmountSchema.nullable().default(null),
    taxCategory: TaxCategorySchema.nullable().default(null),
});

export default TaxSubtotalFormSchema;