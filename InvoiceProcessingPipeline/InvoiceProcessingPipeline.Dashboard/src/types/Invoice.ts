
type PostalAddress = {
    streetName?: string;
    additionalStreetName?: string;
    postalZone?: string;
    countrySubEntity?: string;
    country: string;
    addressLines?: string[];
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
};

export default interface Invoice {
};