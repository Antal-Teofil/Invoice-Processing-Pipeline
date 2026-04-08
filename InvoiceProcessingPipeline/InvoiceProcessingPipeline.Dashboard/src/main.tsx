import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { BrowserRouter, Route, Routes } from 'react-router'
import DashboardLayout from './layout/DashboardLayout'
import HomePage from './pages/HomePage'

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <BrowserRouter>
      <Routes>
        <Route path="/dashboard" element={<DashboardLayout/>}>
          <Route index element={<HomePage/>}></Route>
        </Route>
      </Routes>
    </BrowserRouter>
  </StrictMode>,
)
