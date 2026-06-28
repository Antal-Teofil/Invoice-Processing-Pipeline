import z from "zod";

export const IssueShema = z.object({
    severity: z.enum(['fatal', 'warning'] as const).nullable().default('warning'),
    issueCode: z.string().nullable().default(null),
    description: z.string().nullable().default(null),
    section: z.string().nullable().default(null),
    category: z.string().nullable().default(null),
    confidence: z.number().nullable().default(null)
});
