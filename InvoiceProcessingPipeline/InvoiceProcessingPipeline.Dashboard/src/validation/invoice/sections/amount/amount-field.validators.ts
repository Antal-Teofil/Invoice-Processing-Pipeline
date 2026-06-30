import { nonNegativeNumber } from "../../shared/common-field.validators";

const validateAmount = nonNegativeNumber("Amount");

export const amountFieldValidators = {
  amount: {
    onChange: validateAmount,
    onSubmit: validateAmount,
  },
} as const;
