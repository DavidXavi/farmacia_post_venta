import { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { useAuth } from '../auth/AuthContext'

export function LoginPage() {
  const { login } = useAuth()
  const navigate = useNavigate()
  const [nombreUsuario, setNombreUsuario] = useState('admin')
  const [password, setPassword] = useState('')
  const [error, setError] = useState(null)
  const [cargando, setCargando] = useState(false)

  async function onSubmit(e) {
    e.preventDefault()
    setError(null)
    setCargando(true)
    try {
      await login(nombreUsuario, password)
      navigate('/')
    } catch (err) {
      setError(err.message)
    } finally {
      setCargando(false)
    }
  }

  return (
    <div className="pantalla-centrada">
      <form className="tarjeta" onSubmit={onSubmit}>
        <h1>Sistema POS Farmacia</h1>
        <label>
          Usuario
          <input value={nombreUsuario} onChange={(e) => setNombreUsuario(e.target.value)} required />
        </label>
        <label>
          Contrasena
          <input type="password" value={password} onChange={(e) => setPassword(e.target.value)} required />
        </label>
        {error && <p className="error">{error}</p>}
        <button type="submit" disabled={cargando}>
          {cargando ? 'Ingresando...' : 'Ingresar'}
        </button>
      </form>
    </div>
  )
}
