import type { FieldErrorBag } from "../../shared/validator.types";
import { addFieldError, createFieldErrorBag } from "../../shared/field-error-bag";
import { hasValue } from "../../shared/value.utils";
import {
  isPeppolTaxSchemeId,
  isVatIdCountryPrefixed,
  normalizeSchemeId,
  type PartyTaxSchemeLike,
} from "../../shared/peppol-party.utils";

export function validatePartyTaxSchemeCrossFields(
  taxScheme: PartyTaxSchemeLike | null | undefined,
  path: string,
): FieldErrorBag {
  const errors = createFieldErrorBag();

  if (!taxScheme) {
    return errors;
  }

  if (hasValue(taxScheme.taxSchemeId) && !hasValue(taxScheme.companyId)) {
    addFieldError(errors, `${path}.companyId`, "Tax identifier is required when Tax Scheme ID is set.");
  }

  if (hasValue(taxScheme.companyId) && !hasValue(taxScheme.taxSchemeId)) {
    addFieldError(errors, `${path}.taxSchemeId`, "Tax Scheme ID is required when Tax Identifier is set.");
  }

  if (hasValue(taxScheme.taxSchemeId) && !isPeppolTaxSchemeId(taxScheme.taxSchemeId)) {
    addFieldError(errors, `${path}.taxSchemeId`, "Tax Scheme ID must be VAT when a VAT identifier is provided.");
  }

  if (normalizeSchemeId(taxScheme.taxSchemeId) === "VAT" && hasValue(taxScheme.companyId) && !isVatIdCountryPrefixed(taxScheme.companyId)) {
    addFieldError(
      errors,
      `${path}.companyId`,
      "VAT identifier must start with the country prefix, except Greece may use EL.",
    );
  }

  return errors;
}
