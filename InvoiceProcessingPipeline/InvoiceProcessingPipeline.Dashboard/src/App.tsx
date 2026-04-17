import { BrowserRouter, Route, Routes } from "react-router-dom";
import InvoiceSummaryTable from "./components/InvoiceSummaryTable";

export default function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/invoices" element={<InvoiceSummaryTable/>}></Route>
      </Routes>
    </BrowserRouter>
  );
}