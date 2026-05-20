import { useParams } from "react-router-dom";
import { InvoiceEditorForm } from "../components/InvoiceEditorForm";

export default function InvoiceEditorPage() {
  const { id } = useParams<{ id: string }>();

  if (!id) {
    return <div>No invoice id provided</div>;
  }

  return <InvoiceEditorForm invoiceId={id} />;
}