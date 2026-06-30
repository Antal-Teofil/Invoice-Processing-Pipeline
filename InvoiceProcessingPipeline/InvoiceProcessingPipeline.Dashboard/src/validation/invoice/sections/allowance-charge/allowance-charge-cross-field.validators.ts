import type { FieldErrorBag } from "../../shared/validator.types";
import { addFieldError, createFieldErrorBag } from "../../shared/field-error-bag";
import { formatMoney, isSameMoney, toCents, toNumber } from "../../shared/money.utils";
import { hasValue } from "../../shared/value.utils";

type AllowanceChargeLike = {
  chargeIndicator?: boolean | null;
  multiplierFactorNumeric?: unknown;
  amount?: {
    amount?: unknown;
    currencyCode?: unknown;
  } | null;
  baseAmount?: {
    amount?: unknown;
    currencyCode?: unknown;
  } | null;
};

export function calculateAllowanceChargeAmountCents(
  allowanceCharge: AllowanceChargeLike,
) {
  const baseAmount = toNumber(allowanceCharge.baseAmount?.amount);
  const multiplier = toNumber(allowanceCharge.multiplierFactorNumeric);

  if (baseAmount === null || multiplier === null) {
    return null;
  }

  return Math.round(baseAmount * (multiplier / 100) * 100);
}

export function validateAllowanceChargeCrossFields(
  allowanceCharge: AllowanceChargeLike | null | undefined,
  path: string,
): FieldErrorBag {
  const errors = createFieldErrorBag();

  if (!allowanceCharge) {
    return errors;
  }

  if (hasValue(allowanceCharge.multiplierFactorNumeric) && !hasValue(allowanceCharge.baseAmount?.amount)) {
    addFieldError(
      errors,
      `${path}.baseAmount.amount`,
      "Base Amount is required when Multiplier Factor Numeric is set.",
    );
  }

  const expectedAmountCents = calculateAllowanceChargeAmountCents(allowanceCharge);

  if (expectedAmountCents === null || !hasValue(allowanceCharge.amount?.amount)) {
    return errors;
  }

  const actualAmountCents = toCents(allowanceCharge.amount?.amount);

  if (!isSameMoney(actualAmountCents, expectedAmountCents)) {
    addFieldError(
      errors,
      `${path}.amount.amount`,
      `Amount does not match Base Amount × Multiplier. Expected ${formatMoney(
        expectedAmountCents,
        allowanceCharge.amount?.currencyCode,
      )}.`,
    );
  }

  return errors;
}

export function getAllowanceChargeSignedAmountCents(
  allowanceCharge: AllowanceChargeLike | null | undefined,
) {
  if (!allowanceCharge) {
    return 0;
  }

  const amountCents = toCents(allowanceCharge.amount?.amount);

  return allowanceCharge.chargeIndicator ? amountCents : -amountCents;
}
