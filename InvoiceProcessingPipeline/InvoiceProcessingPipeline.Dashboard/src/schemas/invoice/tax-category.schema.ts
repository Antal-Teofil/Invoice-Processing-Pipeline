import z from "zod";

const TaxCategorySchema = z.object({
    id: z.string().nullable().default(null),
    percent: z.number().nullable().default(null),
    taxScheme: z.string().nullable().default(null),
});

export default TaxCategorySchema;