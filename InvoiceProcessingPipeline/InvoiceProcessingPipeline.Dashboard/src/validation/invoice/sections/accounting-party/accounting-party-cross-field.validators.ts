import type { FieldErrorBag } from "../../shared/validator.types";
import { addFieldError, createFieldErrorBag, mergeFieldErrors } from "../../shared/field-error-bag";
import { hasValue } from "../../shared/value.utils";
import {
  hasSupplierAutoIdentification,
  partyRoleLabel,
  type AccountingPartyLike,
  type PartyRole,
} from "../../shared/peppol-party.utils";
import { validatePartyIdentificationCrossFields } from "../party-identification/party-identification-cross-field.validators";
import { validatePartyLegalEntityCrossFields } from "../party-legal-entity/party-legal-entity-cross-field.validators";
import { validatePartyTaxSchemeCrossFields } from "../party-tax-scheme/party-tax-scheme-cross-field.validators";
import { validatePostalAddressCrossFields } from "../postal-address/postal-address-cross-field.validators";

export function validateAccountingPartyCrossFields(
  party: AccountingPartyLike,
  path: string,
  roleOrLabel: PartyRole | string,
): FieldErrorBag {
  const errors = createFieldErrorBag();

  if (!party) {
    addFieldError(errors, `${path}.partyName`, `${String(roleOrLabel)} party is required.`);
    return errors;
  }

  const role: PartyRole = roleOrLabel === "supplier" || roleOrLabel === "customer"
    ? roleOrLabel
    : String(roleOrLabel).toLowerCase() === "supplier" || String(roleOrLabel).toLowerCase() === "seller"
      ? "supplier"
      : "customer";

  const label = partyRoleLabel(role);

  // BR-06 / BR-07: Seller name and Buyer name are mandatory.
  if (!hasValue(party.partyName)) {
    addFieldError(errors, `${path}.partyName`, `${label} name is required.`);
  }

  // BR-08 / BR-09 and BR-10 / BR-11.
  mergeFieldErrors(
    errors,
    validatePostalAddressCrossFields(party.postalAddress, `${path}.postalAddress`, role),
  );

  party.partyIdentification?.forEach((identification, index) => {
    mergeFieldErrors(
      errors,
      validatePartyIdentificationCrossFields(
        identification,
        `${path}.partyIdentification[${index}]`,
      ),
    );
  });

  party.partyTaxScheme?.forEach((taxScheme, index) => {
    mergeFieldErrors(
      errors,
      validatePartyTaxSchemeCrossFields(
        taxScheme,
        `${path}.partyTaxScheme[${index}]`,
      ),
    );
  });

  mergeFieldErrors(
    errors,
    validatePartyLegalEntityCrossFields(
      party.partyLegalEntity,
      `${path}.partyLegalEntity`,
    ),
  );

  // BR-CO-26: the buyer must be able to identify the supplier automatically.
  if (role === "supplier" && !hasSupplierAutoIdentification(party)) {
    addFieldError(
      errors,
      `${path}.partyIdentification`,
      "Seller identifier, Seller legal registration identifier or Seller VAT identifier is required.",
    );
  }

  return errors;
}
