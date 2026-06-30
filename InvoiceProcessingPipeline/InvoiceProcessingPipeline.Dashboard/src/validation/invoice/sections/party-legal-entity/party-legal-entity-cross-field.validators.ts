import type { FieldErrorBag } from "../../shared/validator.types";
import { addFieldError, createFieldErrorBag } from "../../shared/field-error-bag";
import { hasValue } from "../../shared/value.utils";
import {
  isCommonIso6523IcdSchemeId,
  isIso6523IcdSchemeId,
  type PartyLegalEntityLike,
} from "../../shared/peppol-party.utils";

export function validatePartyLegalEntityCrossFields(
  legalEntity: PartyLegalEntityLike,
  path: string,
): FieldErrorBag {
  const errors = createFieldErrorBag();

  if (!legalEntity) {
    return errors;
  }

  if (hasValue(legalEntity.companySchemeId) && !hasValue(legalEntity.companyId)) {
    addFieldError(
      errors,
      `${path}.companyId`,
      "Legal registration identifier is required when Company Scheme ID is set.",
    );
  }

  if (hasValue(legalEntity.companyId) && !hasValue(legalEntity.companySchemeId)) {
    addFieldError(
      errors,
      `${path}.companySchemeId`,
      "Company Scheme ID is required when Legal Registration Identifier is set.",
    );
  }

  if (hasValue(legalEntity.companySchemeId) && !isIso6523IcdSchemeId(legalEntity.companySchemeId)) {
    addFieldError(
      errors,
      `${path}.companySchemeId`,
      "Company Scheme ID must be a four-digit ISO 6523 ICD code.",
    );
  }

  if (hasValue(legalEntity.companySchemeId) && isIso6523IcdSchemeId(legalEntity.companySchemeId) && !isCommonIso6523IcdSchemeId(legalEntity.companySchemeId)) {
    addFieldError(
      errors,
      `${path}.companySchemeId`,
      "Company Scheme ID must be an accepted four-digit ISO 6523 ICD identifier scheme code.",
    );
  }

  return errors;
}
