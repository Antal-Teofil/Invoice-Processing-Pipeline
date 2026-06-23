import z from "zod";
import PartyIdentificationSchema from "./party-identification.schema";
import PostalAddressSchema from "./postal-address.schema";
import PartyTaxSchemeSchema from "./party-tax-scheme.schema";
import PartyLegalEntitySchema from "./party-legal-entity.schema";
import ContactSchema from "./contact.schema";

const AccountingPartyFormSchema = z.object({
  partyIdentification: z.array(PartyIdentificationSchema).max(2).default([]),
  partyName: z.string().nullable().default(null),
  postalAddress: PostalAddressSchema.nullable().default(null),
  partyTaxScheme: z.array(PartyTaxSchemeSchema).nullable().default(null),
  partyLegalEntity: PartyLegalEntitySchema.nullable().default(null),
  contact: ContactSchema.nullable().default(null),
});

export default AccountingPartyFormSchema;
