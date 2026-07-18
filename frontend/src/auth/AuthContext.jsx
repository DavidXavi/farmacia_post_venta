import { createContext, useContext, useEffect, useState } from 'react'
import { api, setAuthToken } from '../api/client'

const AuthContext = createContext(null)

function loadStoredSession() {
  const raw = localStorage.getItem('posfarmacia.session')
  const session = raw ? JSON.parse(raw) : null
  setAuthToken(session?.token ?? null)
  return session
}

export function AuthProvider({ children }) {
  const [session, setSession] = useState(loadStoredSession)

  useEffect(() => {
    setAuthToken(session?.token ?? null)
  }, [session])

  async function login(nombreUsuario, password) {
    const data = await api.post('/api/auth/login', { nombreUsuario, password })
    localStorage.setItem('posfarmacia.session', JSON.stringify(data))
    setSession(data)
    return data
  }

  function logout() {
    localStorage.removeItem('posfarmacia.session')
    setSession(null)
  }

  function tieneRol(...roles) {
    return session ? roles.some((r) => session.roles.includes(r)) : false
  }

  return (
    <AuthContext.Provider value={{ session, login, logout, tieneRol }}>
      {children}
    </AuthContext.Provider>
  )
}

export function useAuth() {
  return useContext(AuthContext)
}
