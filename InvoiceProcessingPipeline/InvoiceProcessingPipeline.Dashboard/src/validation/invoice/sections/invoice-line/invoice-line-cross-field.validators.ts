import type {
  CommercialInvoiceValidationModel,
  FieldErrorBag,
} from "../../shared/validator.types";
import { addFieldError, createFieldErrorBag, mergeFieldErrors } from "../../shared/field-error-bag";
import { formatMoney, isSameMoney, toCents, toNumber, toNumberOrZero } from "../../shared/money.utils";
import { hasValue } from "../../shared/value.utils";
import { getAllowanceChargeSignedAmountCents, validateAllowanceChargeCrossFields } from "../allowance-charge/allowance-charge-cross-field.validators";
import { validateInvoicePeriodCrossFields } from "../invoice-period/invoice-period-cross-field.validators";

type InvoiceLineLike = CommercialInvoiceValidationModel["invoiceLine"][number];

export function calculateInvoiceLineNetAmountCents(line: InvoiceLineLike) {
  const quantity = toNumber(line.invoicedQuantity?.amount);
  const priceAmount = toNumber(line.price?.priceAmount?.amount);

  if (quantity === null || priceAmount === null) {
    return null;
  }

  const baseQuantity = toNumberOrZero(line.price?.baseQuantity?.amount) || 1;
  const baseLineAmountCents = Math.round((quantity * priceAmount / baseQuantity) * 100);

  const allowanceChargeTotalCents = line.allowanceCharge?.reduce((sum, allowanceCharge) => {
    return sum + getAllowanceChargeSignedAmountCents(allowanceCharge);
  }, 0) ?? 0;

  return baseLineAmountCents + allowanceChargeTotalCents;
}

export function validateInvoiceLineCrossFields(
  line: InvoiceLineLike,
  path: string,
): FieldErrorBag {
  const errors = createFieldErrorBag();

  mergeFieldErrors(
    errors,
    validateInvoicePeriodCrossFields(line.invoicePeriod, `${path}.invoicePeriod`),
  );

  line.allowanceCharge?.forEach((allowanceCharge, index) => {
    mergeFieldErrors(
      errors,
      validateAllowanceChargeCrossFields(
        allowanceCharge,
        `${path}.allowanceCharge[${index}]`,
      ),
    );
  });

  const hasPriceAmount = hasValue(line.price?.priceAmount?.amount);
  const hasInvoicedQuantity = hasValue(line.invoicedQuantity?.amount);
  const hasLineExtensionAmount = hasValue(line.lineExtensionAmount?.amount);

  if (hasPriceAmount && !hasInvoicedQuantity) {
    addFieldError(
      errors,
      `${path}.invoicedQuantity.amount`,
      "Invoiced Quantity is required when Price Amount is set.",
    );
  }

  if (hasInvoicedQuantity && !hasPriceAmount) {
    addFieldError(
      errors,
      `${path}.price.priceAmount.amount`,
      "Price Amount is required when Invoiced Quantity is set.",
    );
  }

  const expectedLineExtensionAmountCents = calculateInvoiceLineNetAmountCents(line);

  if (expectedLineExtensionAmountCents === null || !hasLineExtensionAmount) {
    return errors;
  }

  const actualLineExtensionAmountCents = toCents(line.lineExtensionAmount?.amount);

  if (!isSameMoney(actualLineExtensionAmountCents, expectedLineExtensionAmountCents)) {
    addFieldError(
      errors,
      `${path}.lineExtensionAmount.amount`,
      `Line Extension Amount does not match Quantity × Price +/- line allowances/charges. Expected ${formatMoney(
        expectedLineExtensionAmountCents,
        line.lineExtensionAmount?.currencyCode,
      )}.`,
    );
  }

  return errors;
}
