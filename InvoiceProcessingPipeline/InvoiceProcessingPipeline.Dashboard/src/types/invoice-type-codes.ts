export const INVOICE_TYPE_CODES = [
    { code: '380', label: 'commercial invoice' },
    { code: '381', label: 'credit note' },
    { code: '383', label: 'debit note'},
    { code: '386', label: 'prepayment invoice' }
] as const;

export const CURRENCY_CODES = [
  { code: "EUR", label: "Euro" },
  { code: "HUF", label: "Hungarian Forint" },
  { code: "PLN", label: "Polish Zloty" },
  { code: "CZK", label: "Czech Koruna" },
  { code: "RON", label: "Romanian Leu" },
  { code: "DKK", label: "Danish Krone" },
  { code: "SEK", label: "Swedish Krona" },

  { code: "GBP", label: "British Pound" },
  { code: "CHF", label: "Swiss Franc" },
  { code: "NOK", label: "Norwegian Krone" },
  { code: "ISK", label: "Icelandic Krona" },
  { code: "RSD", label: "Serbian Dinar" },
  { code: "BAM", label: "Bosnia-Herzegovina Convertible Mark" },
  { code: "MKD", label: "Macedonian Denar" },
  { code: "ALL", label: "Albanian Lek" },
  { code: "MDL", label: "Moldovan Leu" },
  { code: "UAH", label: "Ukrainian Hryvnia" },
  { code: "TRY", label: "Turkish Lira" },
  { code: "USD", label: "US Dollar" },
  { code: "CAD", label: "Canadian Dollar" },
] as const;