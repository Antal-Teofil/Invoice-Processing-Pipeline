import type {
  CommercialInvoiceValidationModel,
  FieldErrorBag,
} from "../../shared/validator.types";
import { addFieldError, createFieldErrorBag } from "../../shared/field-error-bag";
import { hasValue, toDateTime } from "../../shared/value.utils";

export function validateInvoiceHeaderCrossFields(
  invoice: CommercialInvoiceValidationModel,
): FieldErrorBag {
  const errors = createFieldErrorBag();

  const issueDateTime = toDateTime(invoice.issueDate);
  const dueDateTime = toDateTime(invoice.dueDate);
  const taxPointDateTime = toDateTime(invoice.taxPointDate);

  if (hasValue(invoice.dueDate) && dueDateTime === null) {
    addFieldError(errors, "dueDate", "Due Date must be a valid date.");
  }

  if (issueDateTime !== null && dueDateTime !== null && dueDateTime < issueDateTime) {
    addFieldError(errors, "dueDate", "Due Date cannot be earlier than Issue Date.");
  }

  if (
    issueDateTime !== null &&
    taxPointDateTime !== null &&
    taxPointDateTime < issueDateTime
  ) {
    addFieldError(errors, "taxPointDate", "Tax Point Date cannot be earlier than Issue Date.");
  }

  if (invoice.taxCurrencyCode && !invoice.documentCurrencyCode) {
    addFieldError(
      errors,
      "documentCurrencyCode",
      "Document Currency Code is required when Tax Currency Code is set.",
    );
  }

  return errors;
}
