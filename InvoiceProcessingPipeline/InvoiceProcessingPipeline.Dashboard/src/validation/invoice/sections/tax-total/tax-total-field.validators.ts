import { nonNegativeNumber } from "../../shared/common-field.validators";

const validateTaxAmount = nonNegativeNumber("Tax Amount");
const validateTaxableAmount = nonNegativeNumber("Taxable Amount");
const validateTaxPercent = nonNegativeNumber("Tax Percent");

export const taxTotalFieldValidators = {
  taxAmount: {
    onChange: validateTaxAmount,
    onSubmit: validateTaxAmount,
  },

  taxableAmount: {
    onChange: validateTaxableAmount,
    onSubmit: validateTaxableAmount,
  },

  taxPercent: {
    onChange: validateTaxPercent,
    onSubmit: validateTaxPercent,
  },
} as const;
