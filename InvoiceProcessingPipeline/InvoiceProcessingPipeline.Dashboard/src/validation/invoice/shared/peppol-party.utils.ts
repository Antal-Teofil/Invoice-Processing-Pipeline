import { hasValue, toNullableString } from "./value.utils";

export type PartyRole = "supplier" | "customer";

export type PartyIdentificationLike = {
  id?: unknown;
  schemeId?: unknown;
};

export type PartyLegalEntityLike = {
  registrationName?: unknown;
  companyId?: unknown;
  companySchemeId?: unknown;
  companyLegalForm?: unknown;
} | null | undefined;

export type PartyTaxSchemeLike = {
  companyId?: unknown;
  taxSchemeId?: unknown;
};

export type PostalAddressLike = {
  streetName?: unknown;
  additionalStreetName?: unknown;
  cityName?: unknown;
  postalZone?: unknown;
  countrySubentity?: unknown;
  addressLine?: unknown;
  countryCode?: unknown;
} | null | undefined;

export type AccountingPartyLike = {
  partyIdentification?: PartyIdentificationLike[] | null;
  partyName?: unknown;
  postalAddress?: PostalAddressLike;
  partyTaxScheme?: PartyTaxSchemeLike[] | null;
  partyLegalEntity?: PartyLegalEntityLike;
  contact?: {
    name?: unknown;
    telephone?: unknown;
    electronicMail?: unknown;
  } | null;
} | null | undefined;

export const PEPPOL_TAX_SCHEME_ID = "VAT";

// A compact allow-list of the ISO 6523 ICD values most commonly used in Peppol BIS Billing.
// Keep the syntax validation below even when extending the list: Peppol scheme identifiers are four digits.
export const COMMON_ISO_6523_ICD_SCHEME_IDS = new Set([
  "0002",
  "0007",
  "0009",
  "0037",
  "0060",
  "0088",
  "0096",
  "0097",
  "0106",
  "0130",
  "0135",
  "0142",
  "0151",
  "0183",
  "0184",
  "0188",
  "0190",
  "0191",
  "0192",
  "0193",
  "0194",
  "0195",
  "0196",
  "0198",
  "0199",
  "0200",
  "0201",
  "0202",
  "0203",
  "0204",
  "0208",
  "0209",
  "0210",
  "0211",
  "0212",
  "0213",
  "0215",
  "0216",
  "0217",
  "0218",
  "0221",
  "0230",
  "0235",
  "0242",
  "0246",
]);

export function normalizeSchemeId(value: unknown) {
  return toNullableString(value)?.toUpperCase() ?? null;
}

export function isIso6523IcdSchemeId(value: unknown) {
  const schemeId = normalizeSchemeId(value);

  if (!schemeId) {
    return false;
  }

  return /^\d{4}$/.test(schemeId);
}

export function isCommonIso6523IcdSchemeId(value: unknown) {
  const schemeId = normalizeSchemeId(value);

  if (!schemeId) {
    return false;
  }

  return COMMON_ISO_6523_ICD_SCHEME_IDS.has(schemeId);
}

export function isPeppolTaxSchemeId(value: unknown) {
  const taxSchemeId = normalizeSchemeId(value);

  if (!taxSchemeId) {
    return true;
  }

  return taxSchemeId === PEPPOL_TAX_SCHEME_ID;
}

export function getPartyTaxSchemeIds(party: AccountingPartyLike) {
  return party?.partyTaxScheme?.filter((taxScheme) => {
    return hasValue(taxScheme.companyId);
  }) ?? [];
}

export function getVatTaxSchemes(party: AccountingPartyLike) {
  return getPartyTaxSchemeIds(party).filter((taxScheme) => {
    return normalizeSchemeId(taxScheme.taxSchemeId) === PEPPOL_TAX_SCHEME_ID;
  });
}

export function hasPartyIdentifier(party: AccountingPartyLike) {
  return Boolean(
    party?.partyIdentification?.some((identifier) => hasValue(identifier.id)),
  );
}

export function hasLegalRegistrationIdentifier(party: AccountingPartyLike) {
  return hasValue(party?.partyLegalEntity?.companyId);
}

export function hasAnyTaxIdentifier(party: AccountingPartyLike) {
  return getPartyTaxSchemeIds(party).length > 0;
}

export function hasVatIdentifier(party: AccountingPartyLike) {
  return getVatTaxSchemes(party).length > 0;
}

export function hasSupplierAutoIdentification(party: AccountingPartyLike) {
  return (
    hasPartyIdentifier(party) ||
    hasLegalRegistrationIdentifier(party) ||
    hasVatIdentifier(party)
  );
}

export function hasPostalAddressContent(address: PostalAddressLike) {
  if (!address) {
    return false;
  }

  return [
    address.streetName,
    address.additionalStreetName,
    address.cityName,
    address.postalZone,
    address.countrySubentity,
    address.addressLine,
    address.countryCode,
  ].some(hasValue);
}

export function hasReadableAddressLine(address: PostalAddressLike) {
  if (!address) {
    return false;
  }

  return [
    address.streetName,
    address.addressLine,
    address.cityName,
    address.postalZone,
  ].some(hasValue);
}

export function getCountryPrefixFromVatId(value: unknown) {
  const text = toNullableString(value)?.toUpperCase() ?? null;

  if (!text) {
    return null;
  }

  const match = text.match(/^[A-Z]{2}/);

  return match?.[0] ?? null;
}

export function isVatIdCountryPrefixed(value: unknown) {
  const prefix = getCountryPrefixFromVatId(value);

  return Boolean(prefix === "EL" || /^[A-Z]{2}$/.test(prefix ?? ""));
}

export function isReverseChargeTaxCategory(value: unknown) {
  return normalizeSchemeId(value) === "AE";
}

export function isStandardRatedTaxCategory(value: unknown) {
  return normalizeSchemeId(value) === "S";
}

export function isZeroRatedTaxCategory(value: unknown) {
  return normalizeSchemeId(value) === "Z";
}

export function isExemptTaxCategory(value: unknown) {
  return normalizeSchemeId(value) === "E";
}

export function isIntraCommunityTaxCategory(value: unknown) {
  return normalizeSchemeId(value) === "K";
}

export function isExportTaxCategory(value: unknown) {
  return normalizeSchemeId(value) === "G";
}

export function isOutOfScopeTaxCategory(value: unknown) {
  return normalizeSchemeId(value) === "O";
}

export function partyRoleLabel(role: PartyRole) {
  return role === "supplier" ? "Seller" : "Buyer";
}
