

export const COUNTRY_CODE_OPTIONS = [
  { code: "HU", label: "Hungary" },
  { code: "PL", label: "Poland" },
  { code: "CZ", label: "Czech Republic" },
  { code: "RO", label: "Romania" },
  { code: "DK", label: "Denmark" },
  { code: "SE", label: "Sweden" },

  { code: "GB", label: "United Kingdom" },
  { code: "CH", label: "Switzerland" },
  { code: "NO", label: "Norway" },
  { code: "IS", label: "Iceland" },
  { code: "RS", label: "Serbia" },
  { code: "BA", label: "Bosnia and Herzegovina" },
  { code: "MK", label: "North Macedonia" },
  { code: "AL", label: "Albania" },
  { code: "MD", label: "Moldova" },
  { code: "UA", label: "Ukraine" },
  { code: "TR", label: "Turkey" },
  { code: "US", label: "United States" },
  { code: "CA", label: "Canada" },
] as const;

export const COUNTRY_CODES = COUNTRY_CODE_OPTIONS.map((country) => country.code) as [string, ...string[]];