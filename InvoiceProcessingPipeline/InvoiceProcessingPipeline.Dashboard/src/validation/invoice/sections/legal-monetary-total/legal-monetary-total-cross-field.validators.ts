import type {
  CommercialInvoiceValidationModel,
  FieldErrorBag,
} from "../../shared/validator.types";
import { addFieldError, createFieldErrorBag } from "../../shared/field-error-bag";
import { formatMoney, isSameMoney, sumCents, toCents } from "../../shared/money.utils";
import { hasValue } from "../../shared/value.utils";

function getRootAllowanceTotalCents(invoice: CommercialInvoiceValidationModel) {
  return sumCents(
    invoice.allowanceCharge
      ?.filter((allowanceCharge) => !allowanceCharge.chargeIndicator)
      .map((allowanceCharge) => allowanceCharge.amount?.amount) ?? [],
  );
}

function getRootChargeTotalCents(invoice: CommercialInvoiceValidationModel) {
  return sumCents(
    invoice.allowanceCharge
      ?.filter((allowanceCharge) => allowanceCharge.chargeIndicator)
      .map((allowanceCharge) => allowanceCharge.amount?.amount) ?? [],
  );
}

function getInvoiceLineExtensionTotalCents(invoice: CommercialInvoiceValidationModel) {
  return sumCents(
    invoice.invoiceLine?.map((line) => line.lineExtensionAmount?.amount) ?? [],
  );
}

function getTaxTotalCents(invoice: CommercialInvoiceValidationModel) {
  return sumCents(
    invoice.taxTotal?.map((taxTotal) => taxTotal.taxAmount?.amount) ?? [],
  );
}

export function validateLegalMonetaryTotalCrossFields(
  invoice: CommercialInvoiceValidationModel,
): FieldErrorBag {
  const errors = createFieldErrorBag();
  const legalMonetaryTotal = invoice.legalMonetaryTotal;
  const currencyCode = invoice.documentCurrencyCode;

  const invoiceLineExtensionTotalCents = getInvoiceLineExtensionTotalCents(invoice);
  const rootAllowanceTotalCents = getRootAllowanceTotalCents(invoice);
  const rootChargeTotalCents = getRootChargeTotalCents(invoice);
  const taxTotalCents = getTaxTotalCents(invoice);

  if (hasValue(legalMonetaryTotal.lineExtensionAmount?.amount)) {
    const actualLineExtensionAmountCents = toCents(legalMonetaryTotal.lineExtensionAmount?.amount);

    if (!isSameMoney(actualLineExtensionAmountCents, invoiceLineExtensionTotalCents)) {
      addFieldError(
        errors,
        "legalMonetaryTotal.lineExtensionAmount.amount",
        `Line Extension Amount must equal the sum of invoice line extension amounts. Expected ${formatMoney(
          invoiceLineExtensionTotalCents,
          currencyCode,
        )}.`,
      );
    }
  }

  if (hasValue(legalMonetaryTotal.allowanceTotalAmount?.amount)) {
    const actualAllowanceTotalCents = toCents(legalMonetaryTotal.allowanceTotalAmount?.amount);

    if (!isSameMoney(actualAllowanceTotalCents, rootAllowanceTotalCents)) {
      addFieldError(
        errors,
        "legalMonetaryTotal.allowanceTotalAmount.amount",
        `Allowance Total Amount must equal the sum of document level allowances. Expected ${formatMoney(
          rootAllowanceTotalCents,
          currencyCode,
        )}.`,
      );
    }
  }

  if (hasValue(legalMonetaryTotal.chargeTotalAmount?.amount)) {
    const actualChargeTotalCents = toCents(legalMonetaryTotal.chargeTotalAmount?.amount);

    if (!isSameMoney(actualChargeTotalCents, rootChargeTotalCents)) {
      addFieldError(
        errors,
        "legalMonetaryTotal.chargeTotalAmount.amount",
        `Charge Total Amount must equal the sum of document level charges. Expected ${formatMoney(
          rootChargeTotalCents,
          currencyCode,
        )}.`,
      );
    }
  }

  const expectedTaxExclusiveAmountCents =
    invoiceLineExtensionTotalCents + rootChargeTotalCents - rootAllowanceTotalCents;

  if (hasValue(legalMonetaryTotal.taxExclusiveAmount?.amount)) {
    const actualTaxExclusiveAmountCents = toCents(legalMonetaryTotal.taxExclusiveAmount?.amount);

    if (!isSameMoney(actualTaxExclusiveAmountCents, expectedTaxExclusiveAmountCents)) {
      addFieldError(
        errors,
        "legalMonetaryTotal.taxExclusiveAmount.amount",
        `Tax Exclusive Amount must equal Line Extension Amount + Charges - Allowances. Expected ${formatMoney(
          expectedTaxExclusiveAmountCents,
          currencyCode,
        )}.`,
      );
    }
  }

  const prepaidAmountCents = toCents(legalMonetaryTotal.prepaidAmount?.amount);
  const roundingAmountCents = toCents(legalMonetaryTotal.payableRoundingAmount?.amount);
  const expectedPayableAmountCents =
    expectedTaxExclusiveAmountCents + taxTotalCents - prepaidAmountCents + roundingAmountCents;

  if (hasValue(legalMonetaryTotal.payableAmount?.amount)) {
    const actualPayableAmountCents = toCents(legalMonetaryTotal.payableAmount?.amount);

    if (!isSameMoney(actualPayableAmountCents, expectedPayableAmountCents)) {
      addFieldError(
        errors,
        "legalMonetaryTotal.payableAmount.amount",
        `Payable Amount must equal Tax Exclusive Amount + Tax Total - Prepaid Amount + Rounding Amount. Expected ${formatMoney(
          expectedPayableAmountCents,
          currencyCode,
        )}.`,
      );
    }
  }

  return errors;
}
