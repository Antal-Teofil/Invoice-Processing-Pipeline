import { Group, Panel, Separator } from "react-resizable-panels";

import { CommercialInvoiceEditorForm } from "../components/InvoiceEditorForm";
import { InvoiceViewer } from "../components/InvoiceViewer";
import { CommercialInvoiceFormSchema } from "../schemas/invoice/invoice.schema";

type EditorPageProperties = {
  invoiceId: string;
  invoiceURL: string;
  invoiceName?: string;
};

export function EditorPage({
  invoiceId,
  invoiceURL,
  invoiceName,
}: EditorPageProperties) {
  const invoice = CommercialInvoiceFormSchema.parse({});

  return (
    <main className="editor-page-shell">
      <Group orientation="horizontal" className="editor-page-panels">
        <Panel defaultSize="60%" minSize="25%" maxSize="55%">
          <section className="editor-viewer-panel">
            <InvoiceViewer
              url={invoiceURL}
              invoiceName={invoiceName ?? `Számla #${invoiceId}`}
            />
          </section>
        </Panel>

        <Separator className="editor-resize-separator">
          <div className="editor-resize-grip" />
        </Separator>

        <Panel defaultSize="40%" minSize="25%">
          <aside className="editor-form-panel">
            <CommercialInvoiceEditorForm invoice={invoice} />
          </aside>
        </Panel>
      </Group>
    </main>
  );
}