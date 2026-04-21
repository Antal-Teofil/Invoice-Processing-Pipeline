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
type Party = {

    name: string;
    address: PostalAddress;
    contact: ContactInformation;
    partyEntity: PartyLegalEntity;
    partyTaxSchemes: Array<PartyTaxScheme>;
};

type InvoiceLine = {

};
export default interface Invoice {

    Id: string;
    accountingCustomerParty: Party;
    accountingSupplierParty: Party;
    issueDate: string;
    dueDate?: string;
    typeCode: string;
    note?: string;
    documentCurrencyCode: string;
    taxPointDate: string;
    lineItems: Array<InvoiceLine>;
    //TO DO: to be extended
};