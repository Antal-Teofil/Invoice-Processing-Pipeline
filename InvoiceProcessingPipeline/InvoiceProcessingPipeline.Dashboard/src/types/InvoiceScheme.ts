import { z } from "zod";

export const PartyTaxScheme = z.object({

    companyId: z.string().nullable().default('ex. Company Kft.'),
    taxScheme: z.string().nullable().default('VAT')
});

export const PostalAddressScheme = z.object({
    streetName: z.string().nullable().default('street name'),
    additionalStreetName: z.string().nullable().default('additional street name'),
    cityName: z.string().nullable().default('ex.: Amsterdam'),
    postalZone: z.string().nullable().default('527192'),
    countrySubEntity: z.string().nullable().default('Region A'),
    addressLine: z.string().nullable().default('address line'),
    countryCode: z.string().nullable().default('DE')
});
export const PartyLegalEntityScheme = z.object({

    registrationName: z.string().nullable().default('buyer full name'),
    companyId: z.string().nullable().default('007')
});
export const ContactInformationScheme = z.object({

    contactName: z.string().nullable().default('Antal Teofil'),
    telephoneNumber: z.string().nullable().default('0786111331'),
    electronicMail: z.email().nullable().default('my@gmail.com')
});
export const PartyScheme = z.object({

    businessName: z.string().nullable().default('ex.: My Business Kft.'),

    address: PostalAddressScheme,
    contact: ContactInformationScheme,
    partyEntity: PartyLegalEntityScheme,
    partyTaxSchemes: PartyTaxScheme,
});

export const ClassifiedTaxCategoryScheme = z.object({
    vatCategoryCodeId: z.string().nullable().default('S'),
    percent: z.coerce.number().nullable().default(0),
    taxScheme: z.string().nullable().default('VAT')
});
export const AllowanceChargeScheme = z.object({
    chargeIndicator: z.boolean().default(true),
    allowanceChargeReasonCode: z.string().nullable().optional().default('allowance charge reason code'),
    allowanceChargeReason: z.string().nullable().optional().default('allowance charge reason'),
    multiplierFactorNumeric: z.coerce.number().optional().nullable().default(0),
    amount: z.coerce.number().nullable().default(0.00),
    amountCurrencyId: z.string().nullable().default('EUR'),
    baseAmount: z.coerce.number().nullable().default(0),
    baseAmountCurrencyId: z.string().nullable().default('EUR'),
});

export const InvoiceItemScheme = z.object({
    description: z.string().nullable().default('item description'),
    name: z.string().nullable().default('item name'),
    sellersItemIdentification: z.string().nullable().default('item stock identification'),
    originCountry: z.string().nullable().default('ex.: Germany'),
    countryCode: z.string().nullable().default('ex.: DE'),
    classifiedTaxCategory: ClassifiedTaxCategoryScheme
});

export const InvoiceLineScheme = z.object({
    lineId: z.string().nullable().default('unique identifier'),
    note: z.string().nullable().default('arbitrary note'),
    invoicedQuantity: z.coerce.number().nullable().default(0.00),
    invoicedQuantityUnitCode: z.string().nullable().default('KGM'),
    lineExtensionAmount: z.coerce.number().nullable().default(0.00), // invoice line net amount
    lineExtensionAmountCurrencyId: z.string().nullable().default('EUR'),
    allowanceCharges: z.array(AllowanceChargeScheme).default([]),
    item: InvoiceItemScheme,
    priceAmount: z.coerce.number().nullable().default(0.00),
    priceAmountCurrencyId: z.string().nullable().default('EUR'),
    baseQuantity: z.coerce.number().nullable().default(0),
    baseQuantityUnitCode: z.string().nullable().default('KGM'),
    allowanceCharge: AllowanceChargeScheme
});

export const TaxTotalScheme = z.object({
    taxAmount: z.coerce.number().nullable().default(0.00),
    taxAmountCurrencyId: z.string().nullable().default('EUR')
});
export const InvoiceScheme  = z.object({

    invoiceId: z.string().nullable().default("invoice id"),
    accountingCustomerParty: PartyScheme,
    accountingSupplierParty: PartyScheme,
    issueDate: z.iso.date().nullable().default('0000-00-00'),
    dueDate: z.iso.date().nullable().default('0000-00-00'),
    typeCode: z.string().nullable().default('380'),
    note: z.string().nullable().default('arbitrary note'),
    documentCurrencyCode: z.string().nullable().default('EUR'),
    taxCurrencyCode: z.string().nullable().default('EUR'),
    taxPointDate: z.iso.date().nullable().default('0000-00-00'),
    lineItems: z.array(InvoiceLineScheme).nullable(),
    allowanceCharge: AllowanceChargeScheme,
    taxTotal: TaxTotalScheme,
});