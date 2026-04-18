import { BrowserRouter, Route, Routes } from "react-router-dom";
import InvoiceSummaryTable from "./components/InvoiceSummaryTable";
import InvoiceDetailsPage from "./pages/InvoiceEditorPage";

export default function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/dashboard" element={<InvoiceSummaryTable/>}></Route>
        <Route path="/invoices/:id" element={<InvoiceDetailsPage/>}></Route>
      </Routes>
    </BrowserRouter>
  );
}