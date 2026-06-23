import z from "zod";


const InvoicePeriodFormSchema = z.object({
    startDate: z.iso.date().nullable().default(null),
    endDate: z.iso.date().nullable().default(null),
    descriptionCode: z.string().nullable().default(null)
});

export default InvoicePeriodFormSchema;