import z from "zod";

const PartyIdentificationSchema = z.object({
    id: z.string().nullable().default(null),
    schemeId: z.string().nullable().default(null)
});

export default PartyIdentificationSchema