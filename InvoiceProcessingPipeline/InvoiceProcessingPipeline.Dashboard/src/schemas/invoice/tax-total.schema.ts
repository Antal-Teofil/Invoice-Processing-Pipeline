import z from "zod";
import AmountSchema from "./amount.schema";
import TaxSubtotalFormSchema from "./tax-subtotal.schema";

const TaxTotalFormSchema = z.object({
    taxAmount: AmountSchema.nullable().default(null),
    taxSubtotal: z.array(TaxSubtotalFormSchema).nullable().default(null),
});

export default TaxTotalFormSchema;