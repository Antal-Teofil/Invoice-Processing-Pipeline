import z from "zod";

const PartyTaxSchemeFormSchema = z.object({
    companyId: z.string().nullable().default(null),
    taxSchemeId: z.string().nullable().default(null)
});

export default PartyTaxSchemeFormSchema;