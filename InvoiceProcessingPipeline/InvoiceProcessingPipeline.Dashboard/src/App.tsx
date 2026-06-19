import { BrowserRouter, Route, Routes } from "react-router-dom";
import HomePage from "./pages/HomePage";
import OverviewPage from "./pages/OverviewPage";

export default function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/home" element={<HomePage/>}></Route>
        <Route path="/dashboard" element={<OverviewPage/>}></Route>
      </Routes>
    </BrowserRouter>
  );
}