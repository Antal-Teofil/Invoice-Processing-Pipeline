import { BrowserRouter, Route, Routes } from "react-router-dom";
import InvoiceSummaryTable from "./components/InvoiceSummaryTable";
import InvoiceEditorPage from "./pages/InvoiceEditorPage";
import InvoiceEditorTestPage from "./pages/InvoiceEditorTestPage";

export default function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/dashboard" element={<InvoiceSummaryTable/>}></Route>
        <Route path="/invoices/:id" element={<InvoiceEditorPage/>}></Route>
        <Route path="/invoice-editor-test" element={<InvoiceEditorTestPage />} />
      </Routes>
    </BrowserRouter>
  );
}