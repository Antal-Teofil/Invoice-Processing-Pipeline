import z from "zod";

export const QuantityFormSchema = z.object({
    amount: z.number().nullable().default(null),
    unitCode: z.string().nullable().default(null)
});
