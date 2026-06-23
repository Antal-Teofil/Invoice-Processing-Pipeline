import z from "zod";

const ContactFormSchema = z.object({
    name: z.string().nullable().default(null),
    telephone: z.string().nullable().default(null),
    electronicMail: z.string().nullable().default(null)
});

export default ContactFormSchema;