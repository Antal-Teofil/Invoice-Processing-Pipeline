import z from "zod";
import { CURRENCY_TYPE_CODES } from "../../shared/constants/currency-code.constant";

const AmountSchema = z.object({
    amount: z.number().nullable().default(null),
    currencyCode: z.enum(CURRENCY_TYPE_CODES).nullable().default(null)
});

export default AmountSchema;