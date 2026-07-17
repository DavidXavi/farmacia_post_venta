import { NavLink, Outlet, useNavigate } from 'react-router-dom'
import { useAuth } from '../auth/AuthContext'

const ENLACES = [
  { to: '/', label: 'Venta (POS)' },
  { to: '/caja', label: 'Caja' },
  { to: '/productos', label: 'Productos' },
  { to: '/lotes', label: 'Lotes' },
  { to: '/clientes', label: 'Clientes' },
  { to: '/recetas', label: 'Recetas' },
  { to: '/reportes', label: 'Reportes' },
  { to: '/auditoria', label: 'Auditoria' },
]

export function AppLayout() {
  const { session, logout } = useAuth()
  const navigate = useNavigate()

  function onLogout() {
    logout()
    navigate('/login')
  }

  return (
    <div className="app-shell">
      <aside className="barra-lateral">
        <h2>POS Farmacia</h2>
        <nav>
          {ENLACES.map((enlace) => (
            <NavLink key={enlace.to} to={enlace.to} end={enlace.to === '/'}>
              {enlace.label}
            </NavLink>
          ))}
        </nav>
        <div className="sesion-info">
          <p>{session?.nombreUsuario}</p>
          <p className="roles">{session?.roles?.join(', ')}</p>
          <button onClick={onLogout}>Cerrar sesion</button>
        </div>
      </aside>
      <main className="contenido">
        <Outlet />
      </main>
    </div>
  )
}
