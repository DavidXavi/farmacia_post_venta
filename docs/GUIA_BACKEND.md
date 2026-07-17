# Guía paso a paso — Backend (.NET 10, Arquitectura Onion)

Esta guía documenta, con los comandos exactos, cómo se creó el backend desde cero y
cómo agregar código nuevo (una entidad, un caso de uso, un controller) respetando la
arquitectura. Todo se ejecuta desde `backend/` salvo que se indique lo contrario.

## 1. Requisitos

- .NET SDK 10 instalado (`dotnet --version` → `10.x`)
- Docker (para levantar Postgres mientras desarrollas)

## 2. Crear la solución y los 4 proyectos

Cada capa del Onion es un **proyecto `.csproj` independiente**, no una carpeta dentro
de un único proyecto. Esto es lo que permite que el compilador imponga la regla de
dependencias (Domain no puede referenciar Infrastructure porque no existe esa
`ProjectReference`).

```bash
mkdir backend/src backend
cd backend

# 1. El archivo de solución
dotnet new sln -n PosFarmacia

# 2. Las 3 bibliotecas de clases (Domain, Application, Infrastructure)
dotnet new classlib -n PosFarmacia.Domain         -o src/PosFarmacia.Domain         --framework net10.0
dotnet new classlib -n PosFarmacia.Application    -o src/PosFarmacia.Application    --framework net10.0
dotnet new classlib -n PosFarmacia.Infrastructure -o src/PosFarmacia.Infrastructure --framework net10.0

# 3. El único proyecto ejecutable: la Web API
dotnet new webapi -n PosFarmacia.Presentation -o src/PosFarmacia.Presentation --framework net10.0

# 4. Registrar los 4 proyectos en la solución
dotnet sln add src/PosFarmacia.Domain/PosFarmacia.Domain.csproj \
               src/PosFarmacia.Application/PosFarmacia.Application.csproj \
               src/PosFarmacia.Infrastructure/PosFarmacia.Infrastructure.csproj \
               src/PosFarmacia.Presentation/PosFarmacia.Presentation.csproj
```

Cada `dotnet new classlib`/`webapi -o <ruta>` crea el `.csproj` **dentro** de esa ruta,
por eso primero se define la carpeta destino (`src/PosFarmacia.Domain`, etc.).

## 3. Conectar las capas (`ProjectReference`)

La dirección de las flechas es la regla de negocio más importante del proyecto:

```
Presentation → Application → Domain
Infrastructure → Application (y transitivamente Domain)
```

```bash
dotnet add src/PosFarmacia.Application/PosFarmacia.Application.csproj \
  reference src/PosFarmacia.Domain/PosFarmacia.Domain.csproj

dotnet add src/PosFarmacia.Infrastructure/PosFarmacia.Infrastructure.csproj \
  reference src/PosFarmacia.Application/PosFarmacia.Application.csproj

dotnet add src/PosFarmacia.Presentation/PosFarmacia.Presentation.csproj \
  reference src/PosFarmacia.Application/PosFarmacia.Application.csproj \
            src/PosFarmacia.Infrastructure/PosFarmacia.Infrastructure.csproj
```

`Presentation` referencia `Infrastructure` únicamente porque `Program.cs` (el
composition root) necesita registrar sus implementaciones en el contenedor de DI — el
código de los controllers nunca importa nada de `Infrastructure` directamente.

## 4. Carpetas internas por capa

```bash
mkdir -p src/PosFarmacia.Domain/{Entities,ValueObjects,Enums,Exceptions,Repositories,Services,Events}
mkdir -p src/PosFarmacia.Application/{UseCases,DTOs,Commands,Queries,Validators,Mappers,Ports}
mkdir -p src/PosFarmacia.Infrastructure/{Persistence,Repositories,Database,Authentication,Insurance,CentralClients,ElectronicBilling,Storage,Auditing}
mkdir -p src/PosFarmacia.Presentation/{Controllers,Endpoints,Middleware}
```

(`Commands`, `Queries`, `Validators`, `Events`, `CentralClients`, `ElectronicBilling`,
`Storage` quedaron como carpetas sin usar en esta iteración porque no había todavía
código que ubicar ahí — CQRS explícito, eventos de dominio y esas integraciones son
funcionalidades opcionales según el enunciado.)

## 5. Paquetes NuGet

```bash
# Infrastructure: EF Core sobre PostgreSQL + herramientas de migraciones + JWT
dotnet add src/PosFarmacia.Infrastructure package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add src/PosFarmacia.Infrastructure package Microsoft.EntityFrameworkCore.Design
dotnet add src/PosFarmacia.Infrastructure package System.IdentityModel.Tokens.Jwt
dotnet add src/PosFarmacia.Infrastructure package Microsoft.Extensions.Options.ConfigurationExtensions

# Application: solo necesita el contrato de DI para exponer AddApplicationUseCases()
dotnet add src/PosFarmacia.Application package Microsoft.Extensions.DependencyInjection.Abstractions

# Presentation: autenticación JWT + mismas versiones de EF Core que Infrastructure
# (si no se fijan las mismas versiones aquí, MSBuild tira warning MSB3277 de conflicto
# de ensamblados porque Microsoft.AspNetCore.OpenApi trae una versión distinta de EF Core)
dotnet add src/PosFarmacia.Presentation package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add src/PosFarmacia.Presentation package Microsoft.EntityFrameworkCore --version 10.0.10
dotnet add src/PosFarmacia.Presentation package Microsoft.EntityFrameworkCore.Relational --version 10.0.10
dotnet add src/PosFarmacia.Presentation package Microsoft.EntityFrameworkCore.Design --version 10.0.10
```

## 6. Herramienta de migraciones y primera migración

```bash
dotnet tool install --global dotnet-ef --version 10.0.10

dotnet ef migrations add InitialCreate \
  --project src/PosFarmacia.Infrastructure \
  --startup-project src/PosFarmacia.Presentation \
  --output-dir Persistence/Migrations
```

`--project` es donde vive el `DbContext` (Infrastructure); `--startup-project` es el
que EF Core arranca para leer la configuración/connection string (Presentation, el
único con `Program.cs`).

## 7. Compilar y correr

```bash
dotnet build                                          # toda la solución
dotnet run --project src/PosFarmacia.Presentation      # levanta la API (necesita Postgres corriendo)
```

---

## 8. Cómo agregar un módulo nuevo (ejemplo end-to-end: "Proveedor")

Este es el recorrido real que se siguió para cada módulo del sistema (Producto, Lote,
Venta, etc.), aplicado a un caso nuevo hipotético para que sirva de plantilla.

### 8.1 Entidad en Domain

`src/PosFarmacia.Domain/Entities/Proveedor.cs`

```csharp
namespace PosFarmacia.Domain.Entities;

public sealed class Proveedor : Entidad
{
    private Proveedor() { } // requerido por EF Core para materializar

    public Proveedor(string nombre, string ruc)
    {
        Nombre = nombre;
        Ruc = ruc;
        Activo = true;
    }

    public string Nombre { get; private set; } = string.Empty;
    public string Ruc { get; private set; } = string.Empty;
    public bool Activo { get; private set; }

    public void Desactivar() => Activo = false;
}
```

Reglas del proyecto para entidades: heredan de `Entidad` (da el `Id` en GUID v7),
constructor privado sin parámetros para EF Core, propiedades con `private set`, y toda
regla de negocio como método (nunca setters públicos sueltos).

### 8.2 Interfaz de repositorio en Domain

`src/PosFarmacia.Domain/Repositories/IProveedorRepository.cs`

```csharp
using PosFarmacia.Domain.Entities;

namespace PosFarmacia.Domain.Repositories;

public interface IProveedorRepository : IRepositorio<Proveedor>;
```

`IRepositorio<T>` (en `IRepositorio.cs`) ya da `ObtenerPorIdAsync`,
`ObtenerTodosAsync` y `AgregarAsync` — solo se declaran métodos *extra* si el módulo
los necesita (búsquedas específicas, por ejemplo).

### 8.3 DTOs y caso de uso en Application

`src/PosFarmacia.Application/DTOs/ProveedorDtos.cs`

```csharp
namespace PosFarmacia.Application.DTOs;

public sealed record RegistrarProveedorRequest(string Nombre, string Ruc);
public sealed record ProveedorResponse(Guid Id, string Nombre, string Ruc, bool Activo);
```

`src/PosFarmacia.Application/Mappers/ProveedorMappers.cs`

```csharp
using PosFarmacia.Application.DTOs;
using PosFarmacia.Domain.Entities;

namespace PosFarmacia.Application.Mappers;

public static class ProveedorMappers
{
    public static ProveedorResponse ToResponse(this Proveedor p) => new(p.Id, p.Nombre, p.Ruc, p.Activo);
}
```

`src/PosFarmacia.Application/UseCases/ProveedorUseCases.cs`

```csharp
using PosFarmacia.Application.DTOs;
using PosFarmacia.Application.Mappers;
using PosFarmacia.Domain.Entities;
using PosFarmacia.Domain.Repositories;

namespace PosFarmacia.Application.UseCases;

public sealed class RegistrarProveedorUseCase(IProveedorRepository proveedores, IUnitOfWork unitOfWork)
{
    public async Task<ProveedorResponse> EjecutarAsync(RegistrarProveedorRequest request, CancellationToken ct = default)
    {
        var proveedor = new Proveedor(request.Nombre, request.Ruc);
        await proveedores.AgregarAsync(proveedor, ct);
        await unitOfWork.GuardarCambiosAsync(ct);
        return proveedor.ToResponse();
    }
}
```

No hace falta registrar la clase a mano en el contenedor de DI: `AddApplicationUseCases()`
(en `PosFarmacia.Application/DependencyInjection.cs`) escanea por reflexión todas las
clases del namespace `PosFarmacia.Application.UseCases` y las registra como `Scoped`.

### 8.4 Configuración EF Core y repositorio en Infrastructure

`src/PosFarmacia.Infrastructure/Persistence/Configurations/CatalogoConfigurations.cs`
(o un archivo nuevo si el módulo lo amerita):

```csharp
public sealed class ProveedorConfiguration : IEntityTypeConfiguration<Proveedor>
{
    public void Configure(EntityTypeBuilder<Proveedor> builder)
    {
        builder.ToTable("proveedores");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Nombre).IsRequired().HasMaxLength(150);
        builder.Property(p => p.Ruc).IsRequired().HasMaxLength(20);
    }
}
```

Agregar el `DbSet` en `PosFarmaciaDbContext.cs`:

```csharp
public DbSet<Proveedor> Proveedores => Set<Proveedor>();
```

`src/PosFarmacia.Infrastructure/Repositories/ProveedorRepository.cs`

```csharp
public sealed class ProveedorRepository(PosFarmaciaDbContext contexto)
    : RepositorioBase<Proveedor>(contexto), IProveedorRepository;
```

`RepositorioBase<T>` ya implementa los 3 métodos de `IRepositorio<T>` usando
`Contexto.Set<T>()` — por eso una entidad de catálogo simple no necesita más código.

Registrar la interfaz en `src/PosFarmacia.Infrastructure/DependencyInjection.cs`:

```csharp
services.AddScoped<IProveedorRepository, ProveedorRepository>();
```

(Esto sí es manual porque cada interfaz de repositorio es distinta y no hay un
patrón de nombres uniforme que se pueda escanear con seguridad.)

### 8.5 Generar y aplicar la migración

```bash
dotnet ef migrations add AgregarProveedor \
  --project src/PosFarmacia.Infrastructure \
  --startup-project src/PosFarmacia.Presentation \
  --output-dir Persistence/Migrations
```

La migración se aplica sola la próxima vez que arranca la API (`Program.cs` llama
`Database.MigrateAsync()` al inicio) — no hace falta correr `dotnet ef database update`
a mano.

### 8.6 Controller en Presentation

`src/PosFarmacia.Presentation/Controllers/ProveedoresController.cs`

```csharp
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PosFarmacia.Application.DTOs;
using PosFarmacia.Application.UseCases;

namespace PosFarmacia.Presentation.Controllers;

[ApiController]
[Route("api/proveedores")]
[Authorize(Roles = "Administrador")]
public sealed class ProveedoresController(RegistrarProveedorUseCase registrarProveedor) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<ProveedorResponse>> Registrar(RegistrarProveedorRequest request, CancellationToken ct) =>
        Ok(await registrarProveedor.EjecutarAsync(request, ct));
}
```

El controller **solo** conoce `PosFarmacia.Application` (DTOs y casos de uso) — nunca
importa `PosFarmacia.Infrastructure` ni `PosFarmacia.Domain.Entities` directamente. Eso
es lo que permite cambiar de motor de base de datos o de ORM sin tocar un solo
controller.

### 8.7 Verificar

```bash
dotnet build
dotnet run --project src/PosFarmacia.Presentation
curl -X POST http://localhost:5231/api/proveedores \
  -H "Authorization: Bearer <token>" -H "Content-Type: application/json" \
  -d '{"nombre":"Distribuidora ABC","ruc":"20123456789"}'
```

## 9. Docker

```bash
docker compose build backend      # build de la imagen (multi-stage: sdk → aspnet)
docker compose up -d db backend   # levanta Postgres + API
docker compose logs backend -f    # ver migraciones/seed aplicándose al iniciar
```

El `Dockerfile` de `backend/` restaura primero solo los `.csproj` (para aprovechar el
cache de capas de Docker cuando no cambian dependencias) y recién después copia todo
`src/` para compilar.
