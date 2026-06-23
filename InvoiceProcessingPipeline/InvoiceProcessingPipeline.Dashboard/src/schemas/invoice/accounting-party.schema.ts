import z from "zod";

import PartyIdentificationSchema from "./party-identification.schema";
import PostalAddressSchema from "./postal-address.schema";
import PartyTaxSchemeSchema from "./party-tax-scheme.schema";
import PartyLegalEntitySchema from "./party-legal-entity.schema";
import ContactSchema from "./contact.schema";
import {
  arrayDefault,
  objectDefault,
} from "../../shared/utility/zod-default.utility";

const AccountingPartyFormSchema = z.object({
  partyIdentification: arrayDefault(PartyIdentificationSchema),

  partyName: z.string().nullable().default(null),

  postalAddress: objectDefault(PostalAddressSchema),

  partyTaxScheme: arrayDefault(PartyTaxSchemeSchema),

  partyLegalEntity: objectDefault(PartyLegalEntitySchema),

  contact: objectDefault(ContactSchema),
});

export default AccountingPartyFormSchema;
