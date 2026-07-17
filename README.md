# Sistema POS Farmacia

Punto de venta para farmacia (ventas, promociones, recetas, convenios de seguro, líneas
de crédito, lotes/FEFO, anulaciones y notas de crédito) construido con **Arquitectura
Onion**. Especificación funcional completa en
[`docs/Sistema_POS_Farmacia_Javier_Geronimo_Jara.docx`](docs/Sistema_POS_Farmacia_Javier_Geronimo_Jara.docx).

## Stack

| Capa | Tecnología |
|---|---|
| Backend | .NET 10, ASP.NET Core Web API, Entity Framework Core |
| Base de datos | PostgreSQL 16 |
| Frontend | React 19 + Vite |
| Auth | JWT + roles |
| Contenedores | Docker / Docker Compose |

## Cómo levantarlo

```bash
cp .env.example .env   # solo la primera vez
docker compose up -d
```

| Servicio | URL |
|---|---|
| **Frontend (abrir en el navegador)** | http://localhost:5173 |
| Backend API | http://localhost:8088 |
| OpenAPI (JSON) | http://localhost:8088/openapi/v1.json |
| PostgreSQL | localhost:5432 |

Al iniciar, el backend aplica las migraciones de EF Core automáticamente y siembra datos
de prueba (roles, usuarios, un local, una caja, un producto y un lote) si la base está vacía.

## Credenciales de prueba

> ⚠️ Son credenciales de **desarrollo/demo**, generadas por el seed automático
> (`SeedData.cs`). No usar en producción ni reutilizar estos valores fuera de este entorno.

| Usuario | Contraseña | Rol |
|---|---|---|
| `admin` | `Admin123!` | Administrador |
| `cajero` | `Cajero123!` | Cajero |
| `quimico` | `Quimico123!` | Químico farmacéutico |

## Arquitectura (Onion)

```
Presentation → Application → Domain
Infrastructure → Application y/o Domain (mediante interfaces)
```

- **Domain** (`backend/src/PosFarmacia.Domain`): entidades, value objects, enums,
  excepciones, interfaces de repositorio y servicios de dominio (FEFO, promociones,
  copago, incentivos, validación de recetas, anulación de ventas). No depende de nada.
- **Application** (`backend/src/PosFarmacia.Application`): casos de uso, DTOs, mappers y
  *ports* (interfaces hacia infraestructura: JWT, hashing, seguro, auditoría). Depende
  solo de Domain.
- **Infrastructure** (`backend/src/PosFarmacia.Infrastructure`): EF Core + Npgsql,
  repositorios concretos, migraciones, JWT, auditoría y el simulador de seguros.
  Implementa las interfaces definidas en Domain/Application.
- **Presentation** (`backend/src/PosFarmacia.Presentation`): controllers REST, middleware
  de manejo de excepciones (mapea excepciones de dominio a 400/404/409/422), JWT bearer +
  autorización por rol.

Regla de dependencias: el dominio no conoce EF Core, HTTP, ni el framework web. Los
casos de uso reciben interfaces (`IRepositorio<T>`, `IUnitOfWork`, `IPasswordHasher`,
etc.), nunca implementaciones concretas.

## Estructura del repositorio

```
backend/
  src/
    PosFarmacia.Domain/
    PosFarmacia.Application/
    PosFarmacia.Infrastructure/
    PosFarmacia.Presentation/
  Dockerfile
frontend/
  src/
    api/         cliente HTTP (fetch + JWT)
    auth/        contexto de sesión y guard de rutas
    pages/       una página por módulo (Venta, Caja, Productos, Lotes, Clientes, Recetas, Reportes, Auditoria)
    layout/      shell con navegación
  Dockerfile
docker-compose.yml
CLAUDE.md          reglas del proyecto para trabajar con Claude Code
```

## Reglas de negocio clave

Caja abierta obligatoria para vender, una promoción por línea (y una sola vez por
comprobante), medicamentos controlados requieren receta aprobada, receta especial
retenida se usa una sola vez, copago según convenio vigente, crédito limitado al saldo
disponible, despacho por FEFO, bloqueo de productos vencidos o en periodo preventivo
(3 meses), anulación solo el mismo día (si no, nota de crédito), y operación de venta
atómica. Detalle completo en el Word y en `CLAUDE.md`.
