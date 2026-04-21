import { z } from "zod";

type PartyTaxScheme = {

    companyId: string;
    taxScheme: string;
};

type PostalAddress = {
    streetName?: string;
    additionalStreetName?: string;
    postalZone?: string;
    countrySubEntity?: string;
    country: string;
    addressLines?: Array<string>;
};
type PartyLegalEntity = {

    registrationName: string;
    companyId?: string;
    companyLegalForm: string;
};
type ContactInformation = {

    contactName?: string;
    telephoneNumber?: string;
    electronicMail?: string;
};
export const PartySchema = z.object({

    name: string;
    address: PostalAddress;
    contact: ContactInformation;
    partyEntity: PartyLegalEntity;
    partyTaxSchemes: Array<PartyTaxScheme>;
});

type InvoiceLine = {

};
export const InvoiceSchema  = z.object({

    invoiceId: z.string().nullable().default("invoice id"),
    accountingCustomerParty: Party;
    accountingSupplierParty: Party;
    issueDate: z.iso.date().nullable().default('0000-00-00'),
    dueDate: z.iso.date().nullable().default('0000-00-00'),
    typeCode: z.string().nullable().default('380'),
    note: z.string().nullable().default('textual note'),
    documentCurrencyCode: z.string().nullable().default('EUR'),
    taxCurrencyCode: z.string().nullable().default('EUR'),
    taxPointDate: z.iso.date().nullable().default('0000-00-00'),
    lineItems: Array<InvoiceLine>;
    //TO DO: to be extended
});