import { optionalEmail } from "../../shared/common-field.validators";
import type { FieldValidatorArgs } from "../../shared/validator.types";
import { hasValue, toNullableString } from "../../shared/value.utils";

function validateTelephone({ value }: FieldValidatorArgs) {
  const text = toNullableString(value);

  if (!text) {
    return undefined;
  }

  if (!/^[+()\d\s.-]{6,32}$/.test(text)) {
    return "Telephone must look like a valid phone number.";
  }

  return undefined;
}

const validateEmail = optionalEmail("Email");

export const contactFieldValidators = {
  name: {},
  telephone: {
    onChange: validateTelephone,
    onSubmit: validateTelephone,
  },
  electronicMail: {
    onChange: validateEmail,
    onSubmit: validateEmail,
  },
} as const;

export function contactHasAnyValue(contact: { name?: unknown; telephone?: unknown; electronicMail?: unknown } | null | undefined) {
  return Boolean(contact && [contact.name, contact.telephone, contact.electronicMail].some(hasValue));
}
