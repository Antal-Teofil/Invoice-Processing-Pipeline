import { required, validDate } from "../../shared/common-field.validators";
import type { FieldValidatorArgs } from "../../shared/validator.types";
import { toDateTime } from "../../shared/value.utils";

const validateInvoiceNumber = required("Invoice Number");
const validateTypeCode = required("Invoice Type Code");
const validateDocumentCurrencyCode = required("Document Currency Code");

function validateIssueDate(args: FieldValidatorArgs) {
  return required("Issue Date")(args) ?? validDate("Issue Date")(args);
}

function validateDueDate({ value, fieldApi }: FieldValidatorArgs) {
  const issueDate = fieldApi.form.getFieldValue("issueDate");

  const issueDateTime = toDateTime(issueDate);
  const dueDateTime = toDateTime(value);

  if (dueDateTime === null) {
    return undefined;
  }

  if (issueDateTime !== null && dueDateTime < issueDateTime) {
    return "Due Date cannot be earlier than Issue Date.";
  }

  return undefined;
}

function validateTaxPointDate({ value, fieldApi }: FieldValidatorArgs) {
  const issueDate = fieldApi.form.getFieldValue("issueDate");

  const issueDateTime = toDateTime(issueDate);
  const taxPointDateTime = toDateTime(value);

  if (taxPointDateTime === null) {
    return undefined;
  }

  if (issueDateTime !== null && taxPointDateTime < issueDateTime) {
    return "Tax Point Date cannot be earlier than Issue Date.";
  }

  return undefined;
}

export const invoiceHeaderFieldValidators = {
  invoiceNumber: {
    onChange: validateInvoiceNumber,
    onSubmit: validateInvoiceNumber,
  },

  issueDate: {
    onChange: validateIssueDate,
    onSubmit: validateIssueDate,
  },

  dueDate: {
    onChangeListenTo: ["issueDate"],
    onChange: validateDueDate,
    onSubmit: validateDueDate,
  },

  typeCode: {
    onChange: validateTypeCode,
    onSubmit: validateTypeCode,
  },

  taxPointDate: {
    onChangeListenTo: ["issueDate"],
    onChange: validateTaxPointDate,
    onSubmit: validateTaxPointDate,
  },

  documentCurrencyCode: {
    onChange: validateDocumentCurrencyCode,
    onSubmit: validateDocumentCurrencyCode,
  },
} as const;
