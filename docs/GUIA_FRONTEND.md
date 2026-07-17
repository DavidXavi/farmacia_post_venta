# Guía paso a paso — Frontend (React + Vite)

Documenta, con los comandos exactos, cómo se creó el frontend desde cero y cómo
agregar una página/componente nuevo siguiendo el mismo patrón que ya usan las 8
páginas existentes. Todo se ejecuta desde `frontend/` salvo que se indique lo
contrario.

## 1. Requisitos

- Node.js 22+ (`node --version`)

## 2. Crear el proyecto

```bash
# desde la raíz del repo
npm create vite@latest frontend -- --template react
cd frontend
npm install
```

Esto genera el scaffold estándar de Vite + React (JS, no TypeScript): `index.html`,
`vite.config.js`, `src/main.jsx`, `src/App.jsx`.

## 3. Instalar dependencias del proyecto

```bash
npm install react-router-dom
```

Es la única dependencia añadida sobre el template base: da el enrutado por rutas
(`/`, `/caja`, `/productos`, ...) y el guard de autenticación. No se agregó ninguna
librería de UI ni de manejo de estado global (Redux/Zustand) — cada página usa
`useState`/`useEffect` de React directamente, es suficiente para el tamaño del
proyecto.

## 4. Estructura creada

```
src/
  api/client.js        fetch wrapper: agrega el token JWT, arma query strings, parsea errores
  auth/
    AuthContext.jsx     contexto de sesión (login/logout), guarda el token en localStorage
    RequireAuth.jsx     componente que redirige a /login si no hay sesión
  layout/
    AppLayout.jsx        shell con la barra lateral de navegación + <Outlet/>
  pages/
    LoginPage.jsx
    VentaPage.jsx         POS: iniciar venta, agregar producto, promociones, pagos, confirmar
    CajaPage.jsx
    ProductosPage.jsx
    LotesPage.jsx
    ClientesPage.jsx
    RecetasPage.jsx
    ReportesPage.jsx
    AuditoriaPage.jsx
  App.jsx                 define las <Route> y las envuelve en <RequireAuth>
  main.jsx                 monta <BrowserRouter><AuthProvider><App/></AuthProvider></BrowserRouter>
```

```bash
mkdir -p src/api src/auth src/layout src/pages
```

## 5. Cómo consumir la API (`src/api/client.js`)

Todo el frontend llama al backend a través de un único wrapper sobre `fetch` (no se
agregó `axios`: el navegador ya trae `fetch`, no hacía falta una dependencia más):

```js
export const api = {
  get: (path, query) => request(path, { method: 'GET', query }),
  post: (path, body) => request(path, { method: 'POST', body }),
  put: (path, body) => request(path, { method: 'PUT', body }),
  patch: (path, body) => request(path, { method: 'PATCH', body }),
}
```

`request()` arma la URL contra `VITE_API_URL` (o `http://localhost:8088` por
defecto), agrega `Authorization: Bearer <token>` si hay sesión activa, y convierte
cualquier respuesta no-2xx en un `throw new Error(mensaje)` — así cada página solo
necesita un `try/catch` alrededor del `await api.post(...)`.

## 6. Autenticación (`src/auth/AuthContext.jsx`)

`AuthProvider` guarda `{ token, usuarioId, nombreUsuario, roles, localId }` en
`localStorage` (clave `posfarmacia.session`) y expone `login()`, `logout()` y
`tieneRol(...roles)` vía `useAuth()`. `RequireAuth` es el componente que se pone
alrededor de las rutas privadas en `App.jsx` y redirige a `/login` si no hay sesión.

---

## 7. Cómo agregar una página/componente nuevo (ejemplo: "Proveedores")

Este es el mismo patrón que se siguió para las 8 páginas existentes.

### 7.1 Crear el componente de página

`src/pages/ProveedoresPage.jsx`

```jsx
import { useEffect, useState } from 'react'
import { api } from '../api/client'

export function ProveedoresPage() {
  const [proveedores, setProveedores] = useState([])
  const [mensaje, setMensaje] = useState(null)
  const [form, setForm] = useState({ nombre: '', ruc: '' })

  useEffect(() => {
    api.get('/api/proveedores').then(setProveedores).catch((e) => setMensaje(e.message))
  }, [])

  function actualizarCampo(campo, valor) {
    setForm((prev) => ({ ...prev, [campo]: valor }))
  }

  async function registrar(e) {
    e.preventDefault()
    setMensaje(null)
    try {
      const nuevo = await api.post('/api/proveedores', form)
      setProveedores((prev) => [...prev, nuevo])
      setForm({ nombre: '', ruc: '' })
    } catch (err) {
      setMensaje(err.message)
    }
  }

  return (
    <section>
      <h1>Proveedores</h1>
      <form className="tarjeta" onSubmit={registrar}>
        <label>
          Nombre
          <input value={form.nombre} onChange={(e) => actualizarCampo('nombre', e.target.value)} required />
        </label>
        <label>
          RUC
          <input value={form.ruc} onChange={(e) => actualizarCampo('ruc', e.target.value)} required />
        </label>
        <button type="submit">Registrar</button>
      </form>
      {mensaje && <p className="aviso">{mensaje}</p>}
      <table>
        <thead><tr><th>Nombre</th><th>RUC</th></tr></thead>
        <tbody>
          {proveedores.map((p) => <tr key={p.id}><td>{p.nombre}</td><td>{p.ruc}</td></tr>)}
        </tbody>
      </table>
    </section>
  )
}
```

Convenciones que siguen todas las páginas del proyecto: un solo componente por
archivo, nombre en `PascalCase` + sufijo `Page`, estado local con `useState`, sin
`.css` por componente (las clases `tarjeta`/`aviso`/`table` ya están en
`src/index.css` y se reusan en todas las páginas).

### 7.2 Agregar la ruta en `App.jsx`

```jsx
import { ProveedoresPage } from './pages/ProveedoresPage'
// ...
<Route path="proveedores" element={<ProveedoresPage />} />
```

### 7.3 Agregar el link de navegación en `layout/AppLayout.jsx`

```jsx
const ENLACES = [
  // ...
  { to: '/proveedores', label: 'Proveedores' },
]
```

### 7.4 Probar en desarrollo

```bash
npm run dev
```

Abre `http://localhost:5173/proveedores` (con el backend corriendo, ver
`../backend/README.md`).

## 8. Build de producción

```bash
npm run build     # genera dist/
npm run preview   # sirve dist/ localmente para probarlo antes de dockerizar
```

## 9. Docker

```bash
docker compose up -d --build frontend
```

El `Dockerfile` es multi-stage: una etapa `node:22-alpine` que corre `npm ci && npm
run build`, y una etapa final `nginx:alpine` que solo copia `dist/` — la imagen final
no lleva Node ni `node_modules`, solo los estáticos. `nginx.conf` agrega
`try_files $uri /index.html;` para que las rutas de React Router (`/productos`,
`/reportes`, etc.) no den 404 al recargar la página directamente.
