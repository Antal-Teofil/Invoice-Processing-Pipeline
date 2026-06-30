import type {
  CommercialInvoiceValidationModel,
  FieldErrorBag,
  TanStackFormErrorResult,
} from "./shared/validator.types";
import { createFieldErrorBag, mergeFieldErrors, toTanStackFormError } from "./shared/field-error-bag";
import { validateAccountingPartyCrossFields } from "./sections/accounting-party/accounting-party-cross-field.validators";
import { validateAllowanceChargeCrossFields } from "./sections/allowance-charge/allowance-charge-cross-field.validators";
import { validateInvoiceHeaderCrossFields } from "./sections/header/header-cross-field.validators";
import { validateInvoiceLineCrossFields } from "./sections/invoice-line/invoice-line-cross-field.validators";
import { validateInvoicePeriodCrossFields } from "./sections/invoice-period/invoice-period-cross-field.validators";
import { validateLegalMonetaryTotalCrossFields } from "./sections/legal-monetary-total/legal-monetary-total-cross-field.validators";
import { validateTaxTotalCrossFields } from "./sections/tax-total/tax-total-cross-field.validators";
import { validateInvoiceCurrencies } from "./invoice-currency.validators";
import { validateInvoiceRequiredFields } from "./invoice-required-field.validators";
import { validateInvoicePartyPeppolRules } from "./invoice-party-peppol.validators";

export function validateInvoiceCrossFields(
  invoice: CommercialInvoiceValidationModel,
): FieldErrorBag {
  const errors = createFieldErrorBag();

  mergeFieldErrors(errors, validateInvoiceHeaderCrossFields(invoice));

  mergeFieldErrors(
    errors,
    validateInvoicePeriodCrossFields(invoice.invoicePeriod, "invoicePeriod"),
  );

  mergeFieldErrors(
    errors,
    validateAccountingPartyCrossFields(
      invoice.accountingSupplierParty,
      "accountingSupplierParty",
      "Supplier",
    ),
  );

  mergeFieldErrors(
    errors,
    validateAccountingPartyCrossFields(
      invoice.accountingCustomerParty,
      "accountingCustomerParty",
      "Customer",
    ),
  );

  invoice.allowanceCharge?.forEach((allowanceCharge, index) => {
    mergeFieldErrors(
      errors,
      validateAllowanceChargeCrossFields(
        allowanceCharge,
        `allowanceCharge[${index}]`,
      ),
    );
  });

  invoice.taxTotal?.forEach((taxTotal, index) => {
    mergeFieldErrors(
      errors,
      validateTaxTotalCrossFields(taxTotal, `taxTotal[${index}]`),
    );
  });

  invoice.invoiceLine?.forEach((line, index) => {
    mergeFieldErrors(
      errors,
      validateInvoiceLineCrossFields(line, `invoiceLine[${index}]`),
    );
  });

  mergeFieldErrors(errors, validateLegalMonetaryTotalCrossFields(invoice));
  mergeFieldErrors(errors, validateInvoiceCurrencies(invoice));
  mergeFieldErrors(errors, validateInvoicePartyPeppolRules(invoice));

  return errors;
}

export function validateInvoiceOnSubmit(
  invoice: CommercialInvoiceValidationModel,
): TanStackFormErrorResult {
  const errors = createFieldErrorBag();

  mergeFieldErrors(errors, validateInvoiceRequiredFields(invoice));
  mergeFieldErrors(errors, validateInvoiceCrossFields(invoice));

  return toTanStackFormError(errors);
}

export const invoiceFormValidators = {
  onDynamic: ({ value }: { value: CommercialInvoiceValidationModel }) => {
    return validateInvoiceOnSubmit(value);
  },

  onSubmit: ({ value }: { value: CommercialInvoiceValidationModel }) => {
    return validateInvoiceOnSubmit(value);
  },
} as const;
