import { Group, Panel, Separator } from "react-resizable-panels";
import { useParams } from "react-router-dom";

import { CommercialInvoiceEditorForm } from "../components/InvoiceEditorForm";
import { InvoiceViewer } from "../components/InvoiceViewer";
import { useInvoice } from "../hooks/invoice-form.hook";

export function EditorPage() {
  const { documentId } = useParams<{ documentId: string }>();

  const {
    data: invoiceForm,
    isPending,
    isError,
    error,
  } = useInvoice(documentId);

  if (!documentId) {
    return <p>Missing document id</p>;
  }

  if (isPending) {
    return <p>Loading invoice...</p>;
  }

  if (isError) {
    return <p>Error: {error.message}</p>;
  }

  if (!invoiceForm) {
    return <p>No invoice data</p>;
  }

  return (
    <main className="editor-page-shell">
      <Group
        key={documentId}
        orientation="horizontal"
        className="editor-page-panels"
      >
        <Panel defaultSize="55%" minSize="25%" maxSize="70%">
          <section className="editor-viewer-panel">
            <InvoiceViewer
              url={invoiceForm.header.documentResourceUri}
              invoiceName={
                invoiceForm.data.invoiceNumber ??
                `Számla #${invoiceForm.header.documentAuditId}`
              }
            />
          </section>
        </Panel>

        <Separator className="editor-resize-separator">
          <div className="editor-resize-grip" />
        </Separator>

        <Panel defaultSize="45%" minSize="45%">
          <aside className="editor-form-panel">
            <CommercialInvoiceEditorForm
              documentId={documentId}
              invoice={invoiceForm.data}
            />
          </aside>
        </Panel>
      </Group>
    </main>
  );
}
