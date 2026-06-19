import z from "zod";
import AmountSchema from "./amount.schema";
import TaxSubtotalSchema from "./tax-subtotal.schema";

const TaxTotalSchema = z.object({
    taxAmount: AmountSchema.nullable().default(null),
    taxSubtotal: z.array(TaxSubtotalSchema).nullable().default(null),
});

export default TaxTotalSchema;