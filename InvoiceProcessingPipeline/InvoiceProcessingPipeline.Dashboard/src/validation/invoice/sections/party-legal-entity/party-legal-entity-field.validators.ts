import type { FieldValidatorArgs } from "../../shared/validator.types";
import { hasValue } from "../../shared/value.utils";
import {
  isCommonIso6523IcdSchemeId,
  isIso6523IcdSchemeId,
} from "../../shared/peppol-party.utils";

function validateCompanyId({ value, fieldApi }: FieldValidatorArgs) {
  const companySchemeId = fieldApi.form.getFieldValue("companySchemeId");

  if (hasValue(companySchemeId) && !hasValue(value)) {
    return "Legal registration identifier is required when Company Scheme ID is set.";
  }

  return undefined;
}

function validateCompanySchemeId({ value, fieldApi }: FieldValidatorArgs) {
  const companyId = fieldApi.form.getFieldValue("companyId");

  if (!hasValue(value)) {
    if (hasValue(companyId)) {
      return "Company Scheme ID is required when Legal Registration Identifier is set.";
    }

    return undefined;
  }

  if (!isIso6523IcdSchemeId(value)) {
    return "Company Scheme ID must be a four-digit ISO 6523 ICD code.";
  }

  if (!isCommonIso6523IcdSchemeId(value)) {
    return "Company Scheme ID must be an accepted four-digit ISO 6523 ICD identifier scheme code.";
  }

  return undefined;
}

export const partyLegalEntityFieldValidators = {
  registrationName: {},

  companyId: {
    onChangeListenTo: ["companySchemeId"],
    onChange: validateCompanyId,
    onSubmit: validateCompanyId,
  },

  companySchemeId: {
    onChangeListenTo: ["companyId"],
    onChange: validateCompanySchemeId,
    onSubmit: validateCompanySchemeId,
  },

  companyLegalForm: {},
} as const;
