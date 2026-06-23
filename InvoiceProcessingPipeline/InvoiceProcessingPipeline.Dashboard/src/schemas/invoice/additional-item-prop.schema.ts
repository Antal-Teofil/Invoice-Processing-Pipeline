import z from "zod";

export const AdditionalItemPropertyFormSchema = z.object({
    name: z.string().nullable().default(null),
    propertyValue: z.string().nullable().default(null)
}); 
