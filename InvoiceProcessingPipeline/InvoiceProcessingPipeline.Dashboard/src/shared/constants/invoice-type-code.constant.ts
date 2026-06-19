export const INVOICE_TYPE_OPTIONS = [
    { code: '380', label: 'commercial invoice' },
    { code: '381', label: 'credit note' },
    { code: '383', label: 'debit note'},
    { code: '386', label: 'prepayment invoice' }
] as const;

export const INVOICE_TYPE_CODES = ['380', '381', '383', '386'] as const;