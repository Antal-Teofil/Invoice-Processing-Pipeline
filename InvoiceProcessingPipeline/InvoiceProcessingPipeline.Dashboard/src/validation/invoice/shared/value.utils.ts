export function isBlank(value: unknown) {
  return value === null || value === undefined || value === "";
}

export function hasValue(value: unknown) {
  return !isBlank(value);
}

export function toNullableString(value: unknown) {
  if (isBlank(value)) {
    return null;
  }

  return String(value).trim();
}

export function toDateTime(value: unknown) {
  if (isBlank(value)) {
    return null;
  }

  const time = new Date(String(value)).getTime();

  return Number.isFinite(time) ? time : null;
}

export function isValidEmail(value: unknown) {
  const text = toNullableString(value);

  if (!text) {
    return true;
  }

  return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(text);
}
