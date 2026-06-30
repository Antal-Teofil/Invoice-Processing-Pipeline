import type {
  CommercialInvoiceValidationModel,
  FieldErrorBag,
} from "./shared/validator.types";
import { addFieldError, createFieldErrorBag } from "./shared/field-error-bag";

function validateCurrency(
  errors: FieldErrorBag,
  fieldPath: string,
  currencyCode: unknown,
  expectedCurrencyCode: unknown,
) {
  if (
    typeof currencyCode !== "string" ||
    currencyCode === "" ||
    typeof expectedCurrencyCode !== "string" ||
    expectedCurrencyCode === ""
  ) {
    return;
  }

  if (currencyCode !== expectedCurrencyCode) {
    addFieldError(
      errors,
      fieldPath,
      `Currency must match Document Currency Code (${expectedCurrencyCode}).`,
    );
  }
}

export function validateInvoiceCurrencies(
  invoice: CommercialInvoiceValidationModel,
): FieldErrorBag {
  const errors = createFieldErrorBag();
  const documentCurrencyCode = invoice.documentCurrencyCode;

  invoice.allowanceCharge?.forEach((allowanceCharge, index) => {
    validateCurrency(
      errors,
      `allowanceCharge[${index}].amount.currencyCode`,
      allowanceCharge.amount?.currencyCode,
      documentCurrencyCode,
    );

    validateCurrency(
      errors,
      `allowanceCharge[${index}].baseAmount.currencyCode`,
      allowanceCharge.baseAmount?.currencyCode,
      documentCurrencyCode,
    );
  });

  invoice.invoiceLine?.forEach((line, index) => {
    validateCurrency(
      errors,
      `invoiceLine[${index}].lineExtensionAmount.currencyCode`,
      line.lineExtensionAmount?.currencyCode,
      documentCurrencyCode,
    );

    validateCurrency(
      errors,
      `invoiceLine[${index}].price.priceAmount.currencyCode`,
      line.price?.priceAmount?.currencyCode,
      documentCurrencyCode,
    );
  });

  validateCurrency(
    errors,
    "legalMonetaryTotal.lineExtensionAmount.currencyCode",
    invoice.legalMonetaryTotal.lineExtensionAmount?.currencyCode,
    documentCurrencyCode,
  );

  validateCurrency(
    errors,
    "legalMonetaryTotal.taxExclusiveAmount.currencyCode",
    invoice.legalMonetaryTotal.taxExclusiveAmount?.currencyCode,
    documentCurrencyCode,
  );

  validateCurrency(
    errors,
    "legalMonetaryTotal.payableAmount.currencyCode",
    invoice.legalMonetaryTotal.payableAmount?.currencyCode,
    documentCurrencyCode,
  );

  return errors;
}
