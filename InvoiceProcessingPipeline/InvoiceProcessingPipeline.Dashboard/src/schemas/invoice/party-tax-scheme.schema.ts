import z from "zod";

const PartyTaxSchemeSchema = z.object({
    companyId: z.string().nullable().default(null),
    taxSchemeId: z.string().nullable().default(null)
});

export default PartyTaxSchemeSchema;