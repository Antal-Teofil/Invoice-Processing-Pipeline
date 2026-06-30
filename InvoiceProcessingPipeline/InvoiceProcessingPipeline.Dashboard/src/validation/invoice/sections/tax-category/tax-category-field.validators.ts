import { nonNegativeNumber } from "../../shared/common-field.validators";
import type { FieldValidatorArgs } from "../../shared/validator.types";
import { hasValue, toNullableString } from "../../shared/value.utils";

const VAT_CATEGORY_IDS = new Set(["S", "Z", "E", "AE", "K", "G", "O", "L", "M", "B"]);
const validateTaxPercent = nonNegativeNumber("Tax Percent");

function validateTaxCategoryId({ value }: FieldValidatorArgs) {
  const id = toNullableString(value)?.toUpperCase();

  if (!id) {
    return undefined;
  }

  if (!VAT_CATEGORY_IDS.has(id)) {
    return "Tax Category ID should use a supported VAT category code, for example S, Z, E, AE, K, G or O.";
  }

  return undefined;
}

function validateTaxScheme({ value }: FieldValidatorArgs) {
  if (!hasValue(value)) {
    return undefined;
  }

  if (toNullableString(value)?.toUpperCase() !== "VAT") {
    return "Tax Scheme should be VAT when a VAT category code is used.";
  }

  return undefined;
}

export const taxCategoryFieldValidators = {
  id: {
    onChange: validateTaxCategoryId,
    onSubmit: validateTaxCategoryId,
  },

  percent: {
    onChange: validateTaxPercent,
    onSubmit: validateTaxPercent,
  },

  taxScheme: {
    onChange: validateTaxScheme,
    onSubmit: validateTaxScheme,
  },
} as const;
