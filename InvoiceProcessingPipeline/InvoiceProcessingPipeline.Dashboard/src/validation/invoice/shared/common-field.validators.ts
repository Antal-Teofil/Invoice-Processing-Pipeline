import type { FieldValidatorArgs } from "./validator.types";
import { hasValue, isValidEmail, toDateTime } from "./value.utils";
import { toNumber } from "./money.utils";

export function required(label: string) {
  return ({ value }: FieldValidatorArgs) => {
    if (hasValue(value)) {
      return undefined;
    }

    return `${label} is required.`;
  };
}

export function optionalEmail(label: string) {
  return ({ value }: FieldValidatorArgs) => {
    if (isValidEmail(value)) {
      return undefined;
    }

    return `${label} must be a valid email address.`;
  };
}

export function nonNegativeNumber(label: string) {
  return ({ value }: FieldValidatorArgs) => {
    const numberValue = toNumber(value);

    if (numberValue === null || numberValue >= 0) {
      return undefined;
    }

    return `${label} must be zero or greater.`;
  };
}

export function positiveNumber(label: string) {
  return ({ value }: FieldValidatorArgs) => {
    const numberValue = toNumber(value);

    if (numberValue === null || numberValue > 0) {
      return undefined;
    }

    return `${label} must be greater than zero.`;
  };
}

export function validDate(label: string) {
  return ({ value }: FieldValidatorArgs) => {
    if (!hasValue(value) || toDateTime(value) !== null) {
      return undefined;
    }

    return `${label} must be a valid date.`;
  };
}
