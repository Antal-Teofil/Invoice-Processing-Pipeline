import type { FieldValidatorArgs } from "../../shared/validator.types";
import { hasValue } from "../../shared/value.utils";
import {
  isPeppolTaxSchemeId,
  isVatIdCountryPrefixed,
  normalizeSchemeId,
} from "../../shared/peppol-party.utils";

function validateTaxIdentifier({ value, fieldApi }: FieldValidatorArgs) {
  const taxSchemeId = fieldApi.form.getFieldValue("taxSchemeId");

  if (hasValue(taxSchemeId) && !hasValue(value)) {
    return "Tax identifier is required when Tax Scheme ID is set.";
  }

  if (normalizeSchemeId(taxSchemeId) === "VAT" && hasValue(value) && !isVatIdCountryPrefixed(value)) {
    return "VAT identifier must start with the country prefix, except Greece may use EL.";
  }

  return undefined;
}

function validateTaxSchemeId({ value, fieldApi }: FieldValidatorArgs) {
  const companyId = fieldApi.form.getFieldValue("companyId");

  if (!hasValue(value)) {
    if (hasValue(companyId)) {
      return "Tax Scheme ID is required when Tax Identifier is set.";
    }

    return undefined;
  }

  if (!isPeppolTaxSchemeId(value)) {
    return "Tax Scheme ID must be VAT when a VAT identifier is provided.";
  }

  return undefined;
}

export const partyTaxSchemeFieldValidators = {
  companyId: {
    onChangeListenTo: ["taxSchemeId"],
    onChange: validateTaxIdentifier,
    onSubmit: validateTaxIdentifier,
  },

  taxSchemeId: {
    onChangeListenTo: ["companyId"],
    onChange: validateTaxSchemeId,
    onSubmit: validateTaxSchemeId,
  },
} as const;
