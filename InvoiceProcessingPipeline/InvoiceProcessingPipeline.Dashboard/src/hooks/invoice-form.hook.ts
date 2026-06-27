import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { fetchCommercialInvoice, submitCommercialInvoice } from "../services/invoice-form.service";
import type { CommercialInvoiceType } from "../types/form/invoice-form.types";

export const invoiceQueryKeys = {
  detail: (documentId?: string) => ["invoice", documentId] as const,
};

export const useInvoice = (documentId?: string) =>
  useQuery({
    queryKey: invoiceQueryKeys.detail(documentId),
    queryFn: () =>
      fetchCommercialInvoice({
        documentId: documentId!,
      }),
    enabled: Boolean(documentId),
  });

export const useSubmitInvoice = (documentId: string) => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (invoice: CommercialInvoiceType) =>
      submitCommercialInvoice({
        documentId,
        invoice,
      }),

    onSuccess: (invoiceForm) => {
      queryClient.setQueryData(
        invoiceQueryKeys.detail(documentId),
        invoiceForm,
      );
    },
  });
}