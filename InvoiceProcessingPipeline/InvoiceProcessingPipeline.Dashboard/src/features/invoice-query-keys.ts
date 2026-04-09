export const invoiceQueryKeys = {
  all: ["invoices"] as const,
  infinite: (pageSize: number) => [...invoiceQueryKeys.all, "infinite", pageSize] as const,
};