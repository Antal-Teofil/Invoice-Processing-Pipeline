import type { FieldErrorBag } from "../../shared/validator.types";
import { addFieldError, createFieldErrorBag } from "../../shared/field-error-bag";
import { hasValue } from "../../shared/value.utils";
import {
  isCommonIso6523IcdSchemeId,
  isIso6523IcdSchemeId,
  type PartyIdentificationLike,
} from "../../shared/peppol-party.utils";

export function validatePartyIdentificationCrossFields(
  identification: PartyIdentificationLike | null | undefined,
  path: string,
): FieldErrorBag {
  const errors = createFieldErrorBag();

  if (!identification) {
    return errors;
  }

  if (hasValue(identification.schemeId) && !hasValue(identification.id)) {
    addFieldError(errors, `${path}.id`, "Party identifier is required when Scheme ID is set.");
  }

  if (hasValue(identification.id) && !hasValue(identification.schemeId)) {
    addFieldError(errors, `${path}.schemeId`, "Scheme ID is required when Party Identifier is set.");
  }

  if (hasValue(identification.schemeId) && !isIso6523IcdSchemeId(identification.schemeId)) {
    addFieldError(errors, `${path}.schemeId`, "Scheme ID must be a four-digit ISO 6523 ICD code.");
  }

  if (hasValue(identification.schemeId) && isIso6523IcdSchemeId(identification.schemeId) && !isCommonIso6523IcdSchemeId(identification.schemeId)) {
    addFieldError(
      errors,
      `${path}.schemeId`,
      "Scheme ID must be an accepted four-digit ISO 6523 ICD identifier scheme code.",
    );
  }

  return errors;
}
