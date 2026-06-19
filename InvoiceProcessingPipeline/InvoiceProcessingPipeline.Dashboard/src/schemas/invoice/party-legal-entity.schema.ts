import z from "zod";

const PartyLegalEntitySchema = z.object({
    registrationName: z.string().nullable().default(null),
    companyId: z.string().nullable().default(null),
    companySchemeId: z.string().nullable().default(null),
    companyLegalForm: z.string().nullable().default(null)
});

export default PartyLegalEntitySchema;