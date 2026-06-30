import type {
  CommercialInvoiceValidationModel,
  FieldErrorBag,
} from "../../shared/validator.types";
import { addFieldError, createFieldErrorBag } from "../../shared/field-error-bag";
import { formatMoney, isSameMoney, sumCents, toCents, toNumber } from "../../shared/money.utils";
import { hasValue } from "../../shared/value.utils";

type TaxTotalLike = CommercialInvoiceValidationModel["taxTotal"][number];

export function validateTaxTotalCrossFields(
  taxTotal: TaxTotalLike,
  path: string,
): FieldErrorBag {
  const errors = createFieldErrorBag();

  taxTotal.taxSubtotal?.forEach((taxSubtotal, index) => {
    const taxSubtotalPath = `${path}.taxSubtotal[${index}]`;
    const taxableAmount = toNumber(taxSubtotal.taxableAmount?.amount);
    const percent = toNumber(taxSubtotal.taxCategory?.percent);
    const actualTaxAmountCents = toCents(taxSubtotal.taxAmount?.amount);

    if (taxableAmount !== null && percent !== null && hasValue(taxSubtotal.taxAmount?.amount)) {
      const expectedTaxAmountCents = Math.round(taxableAmount * (percent / 100) * 100);

      if (!isSameMoney(actualTaxAmountCents, expectedTaxAmountCents)) {
        addFieldError(
          errors,
          `${taxSubtotalPath}.taxAmount.amount`,
          `Tax Amount does not match Taxable Amount × Tax Percent. Expected ${formatMoney(
            expectedTaxAmountCents,
            taxSubtotal.taxAmount?.currencyCode,
          )}.`,
        );
      }
    }

    if (hasValue(taxSubtotal.taxableAmount?.amount) && percent === null) {
      addFieldError(
        errors,
        `${taxSubtotalPath}.taxCategory.percent`,
        "Tax Percent is required when Taxable Amount is set.",
      );
    }
  });

  const subtotalTaxAmountCents = sumCents(
    taxTotal.taxSubtotal?.map((taxSubtotal) => taxSubtotal.taxAmount?.amount) ?? [],
  );

  if (hasValue(taxTotal.taxAmount?.amount)) {
    const actualTotalTaxAmountCents = toCents(taxTotal.taxAmount?.amount);

    if (!isSameMoney(actualTotalTaxAmountCents, subtotalTaxAmountCents)) {
      addFieldError(
        errors,
        `${path}.taxAmount.amount`,
        `Tax Amount must equal the sum of Tax Subtotal amounts. Expected ${formatMoney(
          subtotalTaxAmountCents,
          taxTotal.taxAmount?.currencyCode,
        )}.`,
      );
    }
  }

  return errors;
}
