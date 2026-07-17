import { Route, Routes } from 'react-router-dom'
import { RequireAuth } from './auth/RequireAuth'
import { AppLayout } from './layout/AppLayout'
import { LoginPage } from './pages/LoginPage'
import { VentaPage } from './pages/VentaPage'
import { CajaPage } from './pages/CajaPage'
import { ProductosPage } from './pages/ProductosPage'
import { LotesPage } from './pages/LotesPage'
import { ClientesPage } from './pages/ClientesPage'
import { RecetasPage } from './pages/RecetasPage'
import { ReportesPage } from './pages/ReportesPage'
import { AuditoriaPage } from './pages/AuditoriaPage'

function App() {
  return (
    <Routes>
      <Route path="/login" element={<LoginPage />} />
      <Route
        path="/"
        element={
          <RequireAuth>
            <AppLayout />
          </RequireAuth>
        }
      >
        <Route index element={<VentaPage />} />
        <Route path="caja" element={<CajaPage />} />
        <Route path="productos" element={<ProductosPage />} />
        <Route path="lotes" element={<LotesPage />} />
        <Route path="clientes" element={<ClientesPage />} />
        <Route path="recetas" element={<RecetasPage />} />
        <Route path="reportes" element={<ReportesPage />} />
        <Route path="auditoria" element={<AuditoriaPage />} />
      </Route>
    </Routes>
  )
}

export default App
