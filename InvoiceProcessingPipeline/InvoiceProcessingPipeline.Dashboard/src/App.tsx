import { BrowserRouter, Route, Routes } from "react-router-dom";
import HomePage from "./pages/HomePage";
import OverviewPage from "./pages/OverviewPage";
import { EditorPage } from "./pages/EditorPage";
import "../src/styles/index.css";

export default function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/home" element={<HomePage/>}></Route>
        <Route path="/dashboard" element={<OverviewPage/>}></Route>
        <Route path="/editor" element={<EditorPage/>}></Route>
      </Routes>
    </BrowserRouter>
  );
}