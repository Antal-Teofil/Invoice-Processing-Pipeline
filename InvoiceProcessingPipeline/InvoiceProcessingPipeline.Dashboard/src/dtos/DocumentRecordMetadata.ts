import * as z from "zod";

const AUDIT_STATUSES : ReadonlyArray<string> = [
    "extracted",
    "failed",
    "constraint_violation",
    "under_review",
    "rejected",
    "approved",
    "booked"
];

/*
1. Ez a validacios sema fogja ellenorizni, hogy a bejovo DocumentRecordInformation valoban
megfelelo formatumu es tipusu mezoket tartalmaz.
2. Ebbol a DTO-bol fogjuk kinyerni a szukseges adatokat, ahhzo, hogy megtervezzuk a megfelelo InvoiceCard komponenst
*/
export const DocumentRecordMetadataSchema = z.object({
    auditStatus: z.enum(AUDIT_STATUSES, {error: (issue) => `Unknown audit status: ${JSON.stringify(issue.input)}`}),
    processId: z.guid({error: (issue) => `Invalid process indentifier format: ${JSON.stringify(issue.input)}`}),
    invoiceId: z.string({error: (issue) => `Invalid invoice identifier format: ${JSON.stringify(issue.input)}`}),
    vendorName: z.string({error: (issue) => `Vendor name must be string: ${JSON.stringify(issue.input)}`}),
    phoneNumber: z.e164({error: (issue) => `Invalid phone nuumber: ${JSON.stringify(issue.input)}`}),
    vendorEmailAddress: z.email({error: (issue) => `Invlaid e-mail address format: ${JSON.stringify(issue.input)}`}),
    totalAmount: z.coerce.number({error: `Total amount must be a number!`}).nonnegative({error: `Total Amount must be non-negative!`}).multipleOf(0.01),
    currencyCode: z.string({error: (issue) => `Invalid currency code: ${JSON.stringify(issue.input)}`})
});

export type DocumentRecordMetadata = z.infer<typeof DocumentRecordMetadataSchema>;