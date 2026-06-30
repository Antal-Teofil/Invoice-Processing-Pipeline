import type { FieldErrorBag } from "../../shared/validator.types";
import { addFieldError, createFieldErrorBag } from "../../shared/field-error-bag";
import { hasValue } from "../../shared/value.utils";
import {
  hasPostalAddressContent,
  hasReadableAddressLine,
  partyRoleLabel,
  type PartyRole,
  type PostalAddressLike,
} from "../../shared/peppol-party.utils";

export function validatePostalAddressCrossFields(
  address: PostalAddressLike,
  path: string,
  role: PartyRole,
): FieldErrorBag {
  const errors = createFieldErrorBag();
  const label = partyRoleLabel(role);

  if (!hasPostalAddressContent(address)) {
    addFieldError(errors, `${path}.countryCode`, `${label} postal address is required.`);
    return errors;
  }

  if (!hasValue(address?.countryCode)) {
    addFieldError(errors, `${path}.countryCode`, `${label} country code is required.`);
  }

  // Peppol only makes the country code fatal here. This additional correction rule prevents
  // technically present but unusable addresses in the correction UI.
  if (!hasReadableAddressLine(address)) {
    addFieldError(
      errors,
      `${path}.streetName`,
      `${label} postal address should contain at least Street Name, Address Line, City or Postal Zone.`,
    );
  }

  return errors;
}
