import { nonNegativeNumber, positiveNumber } from "../../shared/common-field.validators";

const validateInvoicedQuantity = positiveNumber("Invoiced Quantity");
const validateLineExtensionAmount = nonNegativeNumber("Line Extension Amount");
const validatePriceAmount = nonNegativeNumber("Price Amount");
const validateBaseQuantity = positiveNumber("Base Quantity");

export const invoiceLineFieldValidators = {
  invoicedQuantity: {
    onChange: validateInvoicedQuantity,
    onSubmit: validateInvoicedQuantity,
  },

  lineExtensionAmount: {
    onChange: validateLineExtensionAmount,
    onSubmit: validateLineExtensionAmount,
  },

  priceAmount: {
    onChange: validatePriceAmount,
    onSubmit: validatePriceAmount,
  },

  baseQuantity: {
    onChange: validateBaseQuantity,
    onSubmit: validateBaseQuantity,
  },
} as const;
