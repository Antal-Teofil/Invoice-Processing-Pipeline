import { nonNegativeNumber } from "../../shared/common-field.validators";

const validateMultiplierFactorNumeric = nonNegativeNumber("Multiplier Factor Numeric");

export const allowanceChargeFieldValidators = {
  multiplierFactorNumeric: {
    onChange: validateMultiplierFactorNumeric,
    onSubmit: validateMultiplierFactorNumeric,
  },
} as const;
