import { nonNegativeNumber } from "../../shared/common-field.validators";

const validateLineExtensionAmount = nonNegativeNumber("Line Extension Amount");
const validateTaxExclusiveAmount = nonNegativeNumber("Tax Exclusive Amount");
const validateAllowanceTotalAmount = nonNegativeNumber("Allowance Total Amount");
const validateChargeTotalAmount = nonNegativeNumber("Charge Total Amount");
const validatePrepaidAmount = nonNegativeNumber("Prepaid Amount");
const validatePayableAmount = nonNegativeNumber("Payable Amount");

export const legalMonetaryTotalFieldValidators = {
  lineExtensionAmount: {
    onChange: validateLineExtensionAmount,
    onSubmit: validateLineExtensionAmount,
  },

  taxExclusiveAmount: {
    onChange: validateTaxExclusiveAmount,
    onSubmit: validateTaxExclusiveAmount,
  },

  allowanceTotalAmount: {
    onChange: validateAllowanceTotalAmount,
    onSubmit: validateAllowanceTotalAmount,
  },

  chargeTotalAmount: {
    onChange: validateChargeTotalAmount,
    onSubmit: validateChargeTotalAmount,
  },

  prepaidAmount: {
    onChange: validatePrepaidAmount,
    onSubmit: validatePrepaidAmount,
  },

  payableAmount: {
    onChange: validatePayableAmount,
    onSubmit: validatePayableAmount,
  },
} as const;
