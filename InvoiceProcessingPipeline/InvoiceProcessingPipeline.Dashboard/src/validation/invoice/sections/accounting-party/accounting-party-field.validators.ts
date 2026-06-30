import { required } from "../../shared/common-field.validators";
import { contactFieldValidators } from "../contact/contact-field.validators";
import { partyIdentificationFieldValidators } from "../party-identification/party-identification-field.validators";
import { partyLegalEntityFieldValidators } from "../party-legal-entity/party-legal-entity-field.validators";
import { partyTaxSchemeFieldValidators } from "../party-tax-scheme/party-tax-scheme-field.validators";
import { postalAddressFieldValidators } from "../postal-address/postal-address-field.validators";

export const accountingPartyFieldValidators = {
  partyName: {
    onChange: required("Party Name"),
    onSubmit: required("Party Name"),
  },

  // Backwards-compatible alias used by the previous generated ZIP.
  contactElectronicMail: contactFieldValidators.electronicMail,

  partyIdentification: partyIdentificationFieldValidators,
  postalAddress: postalAddressFieldValidators,
  partyTaxScheme: partyTaxSchemeFieldValidators,
  partyLegalEntity: partyLegalEntityFieldValidators,
  contact: contactFieldValidators,
} as const;
