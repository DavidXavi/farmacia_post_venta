# POS Farmacia — Frontend

React + Vite. Consume la API REST del backend (`PosFarmacia.Presentation`).

## Requisitos

- Node.js 22+
- Backend corriendo (ver [`../backend/README.md`](../backend/README.md)) o vía
  `docker compose up -d` desde la raíz del repo.

## Levantar en modo desarrollo (sin Docker)

```bash
npm install
npm run dev
```

Abre http://localhost:5173.

Por defecto apunta a la API en `http://localhost:8088` (ver `src/api/client.js`). Para
apuntar a otra URL, define la variable de entorno de Vite antes de correr `dev`/`build`:

```bash
# .env.local (no se commitea)
VITE_API_URL=http://localhost:8088
```

## Build de producción

```bash
npm run build   # genera dist/
npm run preview # sirve dist/ localmente para probarlo
```

## Levantar con Docker

Este proyecto está pensado para correr en contenedores junto al backend y la base de
datos. Desde la raíz del repo:

```bash
docker compose up -d --build frontend
```

Sirve el build de producción vía nginx en http://localhost:5173, con fallback a
`index.html` (`nginx.conf`) para que las rutas de React Router no den 404 al recargar.

## Credenciales de prueba

Ver [`../README.md`](../README.md#credenciales-de-prueba) — usuarios de demo sembrados
por el backend (`admin` / `Admin123!`, etc.).

## Estructura

```
src/
  api/client.js       fetch wrapper (base URL + token JWT + manejo de errores)
  auth/               AuthContext (sesión en localStorage) + RequireAuth (guard de rutas)
  layout/AppLayout    shell con barra lateral de navegación
  pages/              una página por módulo:
    LoginPage, VentaPage (POS), CajaPage, ProductosPage, LotesPage,
    ClientesPage, RecetasPage, ReportesPage, AuditoriaPage
```

No hay estado global (Redux/Zustand): cada página gestiona su propio estado con
`useState`/`useEffect` y llama a la API directamente vía `src/api/client.js`.
