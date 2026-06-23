import z from "zod";

const PartyIdentificationFormSchema = z.object({
    id: z.string().nullable().default(null),
    schemeId: z.string().nullable().default(null)
});

export default PartyIdentificationFormSchema