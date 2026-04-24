import { InvoiceEditorFormContent } from "../components/InvoiceEditorForm";
import { createDefaultInvoice } from "../scheme/InvoiceScheme";

export default function InvoiceEditorTestPage() {
  const defaultInvoice = createDefaultInvoice();

  return <InvoiceEditorFormContent data={defaultInvoice} />;
}