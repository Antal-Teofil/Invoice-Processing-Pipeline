import type {
  CommercialInvoiceValidationModel,
  FieldErrorBag,
} from "./shared/validator.types";
import { addFieldError, createFieldErrorBag } from "./shared/field-error-bag";
import { hasValue } from "./shared/value.utils";

export function validateInvoiceRequiredFields(
  invoice: CommercialInvoiceValidationModel,
): FieldErrorBag {
  const errors = createFieldErrorBag();

  if (!hasValue(invoice.invoiceNumber)) {
    addFieldError(errors, "invoiceNumber", "Invoice Number is required.");
  }

  if (!hasValue(invoice.issueDate)) {
    addFieldError(errors, "issueDate", "Issue Date is required.");
  }

  if (!hasValue(invoice.typeCode)) {
    addFieldError(errors, "typeCode", "Invoice Type Code is required.");
  }

  if (!hasValue(invoice.documentCurrencyCode)) {
    addFieldError(errors, "documentCurrencyCode", "Document Currency Code is required.");
  }

  if (!invoice.invoiceLine || invoice.invoiceLine.length === 0) {
    addFieldError(errors, "invoiceLine", "At least one invoice line is required.");
  }

  invoice.invoiceLine?.forEach((line, index) => {
    if (!hasValue(line.lineId)) {
      addFieldError(errors, `invoiceLine[${index}].lineId`, "Line ID is required.");
    }

    if (!hasValue(line.invoicedQuantity?.amount)) {
      addFieldError(
        errors,
        `invoiceLine[${index}].invoicedQuantity.amount`,
        "Invoiced Quantity is required.",
      );
    }

    if (!hasValue(line.price?.priceAmount?.amount)) {
      addFieldError(
        errors,
        `invoiceLine[${index}].price.priceAmount.amount`,
        "Price Amount is required.",
      );
    }

    if (!hasValue(line.lineExtensionAmount?.amount)) {
      addFieldError(
        errors,
        `invoiceLine[${index}].lineExtensionAmount.amount`,
        "Line Extension Amount is required.",
      );
    }
  });

  if (!hasValue(invoice.legalMonetaryTotal?.payableAmount?.amount)) {
    addFieldError(
      errors,
      "legalMonetaryTotal.payableAmount.amount",
      "Payable Amount is required.",
    );
  }

  return errors;
}
