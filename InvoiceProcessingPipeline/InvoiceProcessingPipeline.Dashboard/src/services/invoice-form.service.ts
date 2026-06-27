import { axiosClient } from "../clients/AxiosClient";
import {
  InvoiceFormSchema,
  type InvoiceFormType,
} from "../dtos/document-form.dto";
import type { CommercialInvoiceType } from "../types/form/invoice-form.types";

export async function fetchCommercialInvoice({
  documentId,
}: {
  documentId: string;
}): Promise<InvoiceFormType> {
  const { data } = await axiosClient.get<unknown>(
    `/verify/${encodeURIComponent(documentId)}`,
  );

  const parsedData = await InvoiceFormSchema.safeParseAsync(data);

  if (!parsedData.success) {
    console.error(parsedData.error);
    throw new Error("Invalid commercial invoice response");
  }

  return parsedData.data;
}

export async function submitCommercialInvoice({
  documentId,
  invoice,
}: {
  documentId: string;
  invoice: CommercialInvoiceType;
}): Promise<InvoiceFormType> {
  const { data } = await axiosClient.put<unknown>(
    `/verify/${encodeURIComponent(documentId)}`,
    invoice,
  );

  const parsedData = await InvoiceFormSchema.safeParseAsync(data);

  if (!parsedData.success) {
    console.error(parsedData.error);
    throw new Error("Invalid commercial invoice submit response");
  }

  return parsedData.data;
}
