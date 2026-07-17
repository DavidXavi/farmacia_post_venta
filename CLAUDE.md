# Sistema POS Farmacia — reglas del proyecto

Fuente de verdad funcional: `docs/Sistema_POS_Farmacia_Javier_Geronimo_Jara.docx`.
Ante cualquier duda de requerimiento, regla de negocio o entidad, releer ese documento antes de asumir.

## Rol

Actúa como ingeniero de software senior. Código correcto, simple y que respete SOLID. Cero sobre-ingeniería, cero abstracciones especulativas.

## Skills activos siempre

En cada tarea de este proyecto, mantener activos:
- `/codebase-memory` — usar los tools del knowledge graph (search_graph, trace_path, get_code_snippet, query_graph, get_architecture, search_code) para explorar código antes de Grep/Read cuando aplique.
- `/gstack`
- `/ponytail:ponytail` (nivel full) — la solución más simple que funcione; nada de boilerplate ni flexibilidad no pedida.

## Arquitectura: Onion obligatoria

Estructura de carpetas (backend, `.NET`):

```
src/
├── Domain/          # entidades, value objects, enums, excepciones, interfaces de repos, servicios de dominio, eventos
├── Application/      # casos de uso, DTOs, commands, queries, validators, mappers, ports (interfaces hacia infra)
├── Infrastructure/    # EF Core, repos concretos, integraciones externas (seguros, central, comprobantes, storage), auth, auditoría
└── Presentation/      # Controllers/Endpoints, middleware, autenticación/autorización HTTP
```

Regla de dependencias (innegociable):
- `Presentation → Application → Domain`
- `Infrastructure → Application y/o Domain` (mediante interfaces definidas en las capas internas)
- `Domain` no depende de nada: sin EF Core, sin ASP.NET, sin JSON/HTTP, sin librerías de UI.
- Los casos de uso (`Application`) no instancian repositorios concretos, solo consumen interfaces.
- Los controladores no acceden a la base de datos ni ejecutan lógica de negocio (promociones, FEFO, recetas, copago, crédito). Esa lógica vive en `Domain`/`Application`, nunca solo en el frontend.
- Toda validación de stock/negocio se reconfirma en el servidor al confirmar la venta, aunque ya se haya validado en pantalla.

## Reglas de negocio clave (ver Word para el detalle completo)

- Caja debe estar abierta antes de vender.
- Una promoción como máximo por línea de venta; una promoción no se repite en el mismo comprobante.
- Medicamento controlado exige receta válida y aprobada antes de dispensar.
- Receta especial retenida: se usa una sola vez y queda retenida tras la venta.
- Copago se calcula solo si el convenio está activo y vigente; validar por línea/producto.
- Compra a crédito exige DNI y no puede superar el saldo disponible.
- Despacho de lotes por FEFO (vencimiento más cercano primero); nunca vender lotes vencidos o dentro del periodo preventivo (3 meses).
- Venta facturada nunca se borra físicamente; anulación solo el mismo día, después se emite nota de crédito.
- Venta, pago, comprobante y descuento de stock se confirman como una sola operación atómica.
- Operaciones sensibles (anulaciones, notas de crédito, cambios de precio, ajustes de stock, validación de recetas, cambios de promoción) quedan auditadas.

## Stack técnico

- Backend: **.NET 10**, ASP.NET Core Web API.
- Frontend: **React** (última versión estable), no Angular (se reemplaza la propuesta del Word).
- Persistencia: Entity Framework Core + SQL Server o PostgreSQL.
- Auth: JWT + control de acceso por rol.
- Documentación API: Swagger/OpenAPI.
- Tests: xUnit + Moq (unitarias sin tocar BD; de integración con BD/servicios simulados).
- Contenedores: Docker + docker-compose (backend, frontend, BD, mocks de servicios externos si aplica).
- Logging técnico: Serilog u equivalente.
- Secretos y cadenas de conexión: solo por variables de entorno, nunca hardcodeados en el repo.

## Docker obligatorio

- `backend/` y `frontend/` son proyectos independientes, cada uno con su propio `Dockerfile`.
- Ambos se levantan juntos con la base de datos vía `docker-compose.yml` en la raíz (`docker compose up -d`). Nunca asumir que el backend o frontend corren directo en el host como flujo principal — el flujo soportado es contenedores.
- Cualquier servicio nuevo (mocks de seguros, central, etc.) se agrega como servicio adicional en `docker-compose.yml`, no como proceso paralelo fuera de Docker.
- Secretos y cadenas de conexión van por variables de entorno (`.env`, ignorado por git) usando `.env.example` como plantilla. Nunca hardcodear credenciales en `docker-compose.yml`, `appsettings.json` ni en el código.

## Límites de código

- Ningún archivo debe superar **1000 líneas**. Si una clase/archivo se acerca al límite, dividir por responsabilidad (SRP) en vez de seguir agregando.
- Preferir métodos y clases pequeños con una sola razón para cambiar.
- Aplicar los 5 principios SOLID de forma consciente, especialmente inversión de dependencias (interfaces en capas internas, implementaciones en infraestructura) y segregación de interfaces (no interfaces "todo en uno").

## Restricciones explícitas (del Word, sección 16)

No está permitido:
- Lógica de negocio en controladores.
- Acceso directo a BD desde frontend o controladores.
- Usar entidades de EF Core como único modelo de dominio.
- Dominio dependiente de framework.
- Repositorios concretos usados dentro de casos de uso.
- Promociones calculadas solo en el frontend.
- Confiar en el stock de pantalla sin revalidar al confirmar.
- Reutilización de receta retenida por condición de carrera.
- Borrar/modificar ventas facturadas sin trazabilidad.
- Credenciales o claves en el repositorio.
- Entregar código sin pruebas.
