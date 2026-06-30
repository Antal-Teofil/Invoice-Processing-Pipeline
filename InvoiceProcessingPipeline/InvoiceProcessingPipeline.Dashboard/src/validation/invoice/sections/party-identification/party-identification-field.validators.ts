import type { FieldValidatorArgs } from "../../shared/validator.types";
import { hasValue } from "../../shared/value.utils";
import {
  isCommonIso6523IcdSchemeId,
  isIso6523IcdSchemeId,
} from "../../shared/peppol-party.utils";

function validatePartyIdentifier({ value, fieldApi }: FieldValidatorArgs) {
  const schemeId = fieldApi.form.getFieldValue("schemeId");

  if (hasValue(schemeId) && !hasValue(value)) {
    return "Party identifier is required when Scheme ID is set.";
  }

  return undefined;
}

function validateSchemeId({ value, fieldApi }: FieldValidatorArgs) {
  const id = fieldApi.form.getFieldValue("id");

  if (!hasValue(value)) {
    if (hasValue(id)) {
      return "Scheme ID is required when Party Identifier is set.";
    }

    return undefined;
  }

  if (!isIso6523IcdSchemeId(value)) {
    return "Scheme ID must be a four-digit ISO 6523 ICD code.";
  }

  if (!isCommonIso6523IcdSchemeId(value)) {
    return "Scheme ID must be an accepted four-digit ISO 6523 ICD identifier scheme code.";
  }

  return undefined;
}

export const partyIdentificationFieldValidators = {
  id: {
    onChangeListenTo: ["schemeId"],
    onChange: validatePartyIdentifier,
    onSubmit: validatePartyIdentifier,
  },

  schemeId: {
    onChangeListenTo: ["id"],
    onChange: validateSchemeId,
    onSubmit: validateSchemeId,
  },
} as const;
