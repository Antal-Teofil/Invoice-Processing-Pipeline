import { PDFViewer, ZoomMode } from "@embedpdf/react-pdf-viewer";

type InvoiceViewerProperties = {
  url: string;
  invoiceName: string;
};

export function InvoiceViewer({ url, invoiceName }: InvoiceViewerProperties) {
  return (
    <div className="invoice-viewer-shell">
      <header className="invoice-viewer-header">
        <div className="invoice-viewer-title-row">
          <div className="invoice-viewer-pdf-badge" aria-hidden="true">
            PDF
          </div>

          <div className="invoice-viewer-title-content">
            <p className="invoice-viewer-eyebrow">
              Számla előnézet
            </p>

            <h1 className="invoice-viewer-title">
              {invoiceName}
            </h1>
          </div>
        </div>

        <span className="invoice-viewer-status">
          Dokumentum előnézet
        </span>
      </header>

      <div className="invoice-viewer-surface">
        {!url ? (
          <div className="invoice-viewer-empty-state">
            <div className="invoice-viewer-empty-icon" aria-hidden="true">
              📄
            </div>

            <h2 className="invoice-viewer-empty-title">
              A dokumentum előnézete még nem érhető el
            </h2>

            <p className="invoice-viewer-empty-description">
              A számla PDF-előnézete automatikusan megjelenik, amint a dokumentum elkészült.
            </p>
          </div>
        ) : (
          <div className="invoice-viewer-pdf-frame">
            <PDFViewer
              config={{
                src: url,
                theme: {
                  preference: "light",
                },
                tabBar: "never",
                zoom: {
                  defaultZoomLevel: ZoomMode.FitWidth,
                  minZoom: 0.5,
                  maxZoom: 3,
                },
                disabledCategories: [
                  "annotation",
                  "form",
                  "redaction",
                  "document-open",
                  "document-close",
                  "document-print",
                  "document-capture",
                  "document-export",
                  "document-fullscreen",
                  "document-protect",
                  "panel",
                  "tools",
                  "selection",
                  "history",
                  "insert",
                  "rotate",
                  "spread",
                  "navigation",
                ],
              }}
            />
          </div>
        )}
      </div>
    </div>
  );
}