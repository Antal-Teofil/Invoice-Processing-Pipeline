import { isBlank } from "./value.utils";

export function toNumber(value: unknown): number | null {
  if (isBlank(value)) {
    return null;
  }

  const numberValue = Number(value);

  return Number.isFinite(numberValue) ? numberValue : null;
}

export function toNumberOrZero(value: unknown): number {
  return toNumber(value) ?? 0;
}

export function toCents(value: unknown): number {
  return Math.round(toNumberOrZero(value) * 100);
}

export function centsToAmount(cents: number): number {
  return cents / 100;
}

export function multiplyToCents(...values: unknown[]): number {
  const result = values.reduce<number>((sum, value) => {
    return sum * toNumberOrZero(value);
  }, 1);

  return Math.round(result * 100);
}

export function isSameMoney(actualCents: number, expectedCents: number): boolean {
  return Math.abs(actualCents - expectedCents) <= 1;
}

export function formatMoney(cents: number, currencyCode?: unknown): string {
  const currency = typeof currencyCode === "string" && currencyCode
    ? currencyCode
    : "";

  return `${(cents / 100).toFixed(2)}${currency ? ` ${currency}` : ""}`;
}

export function sumCents(values: unknown[]): number {
  return values.reduce<number>((sum, value) => {
    return sum + toCents(value);
  }, 0);
}
