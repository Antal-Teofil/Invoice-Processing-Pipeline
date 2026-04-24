import { z } from "zod";

export const PartyTaxScheme = z.object({

    companyId: z.string().default('ex. Company Kft.'),
    taxScheme: z.string().default('VAT')
});

export const PostalAddressScheme = z.object({
    streetName: z.string().default('street name'),
    additionalStreetName: z.string().default('additional street name'),
    cityName: z.string().default('ex.: Amsterdam'),
    postalZone: z.string().default('527192'),
    countrySubEntity: z.string().default('Region A'),
    addressLine: z.string().default('address line'),
    countryCode: z.string().default('DE')
});
export const PartyLegalEntityScheme = z.object({

    registrationName: z.string().default('buyer full name'),
    companyId: z.string().default('007')
});
export const ContactInformationScheme = z.object({

    contactName: z.string().default('Antal Teofil'),
    telephoneNumber: z.string().default('0786111331'),
    electronicMail: z.email().default('my@gmail.com')
});
export const PartyScheme = z.object({

    businessName: z.string().default('ex.: My Business Kft.'),

    address: PostalAddressScheme,
    contact: ContactInformationScheme,
    partyEntity: PartyLegalEntityScheme,
    partyTaxSchemes: PartyTaxScheme,
});

export const ClassifiedTaxCategoryScheme = z.object({
    vatCategoryCodeId: z.string().default('S'),
    percent: z.coerce.number().default(0),
    taxScheme: z.string().default('VAT')
});
export const AllowanceChargeScheme = z.object({
    chargeIndicator: z.boolean().default(true),
    allowanceChargeReasonCode: z.string().default('allowance charge reason code'),
    allowanceChargeReason: z.string().default('allowance charge reason'),
    multiplierFactorNumeric: z.coerce.number().default(0),
    amount: z.coerce.number().default(0.00),
    amountCurrencyId: z.string().default('EUR'),
    baseAmount: z.coerce.number().default(0),
    baseAmountCurrencyId: z.string().default('EUR'),
});

export const InvoiceItemScheme = z.object({
    description: z.string().default('item description'),
    name: z.string().default('item name'),
    sellersItemIdentification: z.string().default('item stock identification'),
    originCountry: z.string().default('ex.: Germany'),
    countryCode: z.string().default('ex.: DE'),
    classifiedTaxCategory: ClassifiedTaxCategoryScheme
});

export const InvoiceLineScheme = z.object({
    lineId: z.string().default('unique identifier'),
    note: z.string().default('arbitrary note'),
    invoicedQuantity: z.coerce.number().default(0.00),
    invoicedQuantityUnitCode: z.string().default('KGM'),
    lineExtensionAmount: z.coerce.number().default(0.00), // invoice line net amount
    lineExtensionAmountCurrencyId: z.string().default('EUR'),
    allowanceCharges: z.array(AllowanceChargeScheme).default([]),
    item: InvoiceItemScheme,
    priceAmount: z.coerce.number().default(0.00),
    priceAmountCurrencyId: z.string().default('EUR'),
    baseQuantity: z.coerce.number().default(0),
    baseQuantityUnitCode: z.string().default('KGM'),
    allowanceCharge: AllowanceChargeScheme
});

export const TaxTotalScheme = z.object({
    taxAmount: z.coerce.number().default(0.00),
    taxAmountCurrencyId: z.string().default('EUR')
});
export const InvoiceScheme  = z.object({

    invoiceId: z.string().default('unknown'),
    accountingCustomerParty: PartyScheme,
    accountingSupplierParty: PartyScheme,
    issueDate: z.iso.date().default('0000-00-00'),
    dueDate: z.iso.date().default('0000-00-00'),
    typeCode: z.string().default('380'),
    note: z.string().default('arbitrary note'),
    documentCurrencyCode: z.string().default('EUR'),
    taxCurrencyCode: z.string().default('EUR'),
    taxPointDate: z.iso.date().default('0000-00-00'),
    lineItems: z.array(InvoiceLineScheme).min(1),
    allowanceCharge: AllowanceChargeScheme,
    taxTotal: TaxTotalScheme,
});

