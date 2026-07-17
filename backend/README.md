# POS Farmacia — Backend

.NET 10 / ASP.NET Core Web API en Arquitectura Onion. Ver la explicación completa de
capas en el [README de la raíz](../README.md#arquitectura-onion).

## Requisitos

- .NET SDK 10
- PostgreSQL 16 (local o vía Docker) — no se necesita crear la base a mano, el backend
  aplica las migraciones y siembra datos de prueba automáticamente al iniciar.

## Levantar en modo desarrollo (sin Docker)

1. Levanta solo la base de datos (más simple que instalar Postgres local):

   ```bash
   # desde la raíz del repo
   cp .env.example .env   # si no lo hiciste ya
   docker compose up -d db
   ```

2. Ajusta `src/PosFarmacia.Presentation/appsettings.json` si tu Postgres no corre en
   `localhost:5432` con las credenciales por defecto (`posfarmacia` / `change-me`), o
   sobreescribe por variables de entorno (`ConnectionStrings__Default`, `Jwt__Key`, etc.).

3. Corre la API:

   ```bash
   dotnet run --project src/PosFarmacia.Presentation
   ```

   Al iniciar: aplica migraciones EF Core pendientes y siembra roles, usuarios de
   prueba, un local, una caja, un producto y un lote si la base está vacía.

4. La API queda en `http://localhost:5231` (perfil `http` de `launchSettings.json`;
   revisa la consola por si el SDK asigna otro puerto). Prueba con:

   ```bash
   curl -X POST http://localhost:5231/api/auth/login \
     -H "Content-Type: application/json" \
     -d '{"nombreUsuario":"admin","password":"Admin123!"}'
   ```

Credenciales de prueba: ver [`../README.md`](../README.md#credenciales-de-prueba).

## Levantar con Docker

Desde la raíz del repo:

```bash
docker compose up -d --build backend
```

Expuesto en `http://localhost:8088` (ver `docker-compose.yml`; internamente el
contenedor escucha en el puerto 8080).

## Compilar y correr pruebas

```bash
dotnet build
```

(Aún no hay proyecto de pruebas — agregar `dotnet new xunit` en `tests/` cuando se
implementen las pruebas unitarias/integración descritas en la especificación.)

## Migraciones de EF Core

Las migraciones se aplican automáticamente al iniciar (`Program.cs`), pero si necesitas
generarlas o inspeccionarlas manualmente:

```bash
dotnet tool install --global dotnet-ef   # una sola vez
dotnet ef migrations add NombreMigracion \
  --project src/PosFarmacia.Infrastructure \
  --startup-project src/PosFarmacia.Presentation \
  --output-dir Persistence/Migrations
```

## Estructura de la solución

```
PosFarmacia.slnx
src/
  PosFarmacia.Domain/          entidades, value objects, enums, excepciones,
                                interfaces de repositorio, servicios de dominio
  PosFarmacia.Application/     casos de uso, DTOs, mappers, ports (interfaces
                                hacia infraestructura)
  PosFarmacia.Infrastructure/  EF Core + Npgsql, repositorios concretos,
                                migraciones, JWT, hashing, auditoría, seed
  PosFarmacia.Presentation/    controllers REST, middleware de excepciones,
                                Program.cs (composition root)
```

Cada capa es un proyecto `.csproj` independiente (no carpetas dentro de un único
proyecto), de forma que el compilador impone la regla de dependencias: `Domain` no
puede referenciar `Infrastructure` ni `Presentation` porque no existe esa
`ProjectReference`.

## Variables de configuración relevantes

| Clave | Uso |
|---|---|
| `ConnectionStrings__Default` | Cadena de conexión a PostgreSQL |
| `Jwt__Key` / `Jwt__Issuer` / `Jwt__Audience` | Firma y validación de JWT |
| `Cors__OrigenesPermitidos__0` | Origen permitido para CORS (el frontend) |

En Docker se inyectan desde `.env` (ver `docker-compose.yml`); en desarrollo local se
leen de `appsettings.json` / `appsettings.Development.json` o variables de entorno.
