import z from "zod";


const InvoicePeriodSchema = z.object({
    startDate: z.iso.date().nullable().default(null),
    endDate: z.iso.date().nullable().default(null),
    descriptionCode: z.string().nullable().default(null)
});

export default InvoicePeriodSchema;