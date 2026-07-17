# Prompt para Gemini — Diagrama de Arquitectura Onion (POS Farmacia)

Copia y pega esto en Gemini (generación de imágenes):

---

Genera un diagrama técnico tipo infografía que explique la **Arquitectura Onion (Cebolla)**
del backend de un Sistema POS para Farmacia. Debe verse como un diagrama de arquitectura
de software profesional, para una sustentación académica, no una ilustración artística.

**Composición general:** 4 anillos concéntricos (como una cebolla vista en corte),
del centro hacia afuera, cada uno en un color distinto y con su nombre bien legible:

1. **Centro — DOMAIN (Dominio)** — color más oscuro/núcleo, por ejemplo azul marino.
   Etiqueta: "Domain — no depende de nada". Dentro de este anillo, listar en texto
   pequeño: Entidades (Venta, Producto, Lote, Cliente, Receta, Promocion, LineaCredito,
   ConvenioSeguro), Value Objects (Dinero, Cantidad, Dni, FechaVencimiento), Servicios de
   dominio (AsignadorLotesFEFO, EvaluadorPromociones, ValidadorReceta, CalculadorCopago),
   Interfaces de repositorio.

2. **Segundo anillo — APPLICATION (Aplicación)** — color intermedio, por ejemplo azul
   medio/teal. Etiqueta: "Application — casos de uso". Listar: Casos de uso
   (IniciarVenta, AgregarProductoAVenta, ConfirmarVenta, AnularVenta, ValidarReceta),
   DTOs, Ports (interfaces hacia infraestructura: IPasswordHasher, ISeguroClient,
   IAuditService).

3. **Tercer anillo — INFRASTRUCTURE (Infraestructura)** — color distinto, por ejemplo
   naranja o gris, posicionado de forma que se note que "entra" desde afuera hacia
   Application/Domain mediante flechas, no que forma parte del flujo central. Etiqueta:
   "Infrastructure — implementa las interfaces". Listar: Entity Framework Core +
   PostgreSQL, Repositorios concretos, JWT / Autenticación, Auditoría.

4. **Anillo exterior — PRESENTATION (Presentación)** — color más claro/externo, por
   ejemplo verde o celeste claro. Etiqueta: "Presentation — expone la API". Listar:
   Controllers REST (VentasController, ProductosController, RecetasController),
   Middleware de excepciones, JWT Bearer + Autorización por rol.

**Flechas de dependencia (muy importante, deben ser explícitas y legibles):**
- Una flecha grande curva que va desde el anillo Presentation, atravesando Application,
  hacia Domain, con la etiqueta "Presentation → Application → Domain".
- Una flecha distinta (de otro color, por ejemplo roja o punteada) que va desde el
  anillo Infrastructure hacia adentro (hacia Application y Domain), con la etiqueta
  "Infrastructure → Application / Domain (vía interfaces)".
- Debe quedar visualmente claro que **todas las flechas apuntan hacia el centro**: el
  Dominio nunca depende de las capas externas.

**Fuera del anillo exterior, a la derecha o abajo**, agregar una caja pequeña separada
representando el **Frontend React (SPA)**, conectada con una flecha HTTP/REST hacia el
anillo Presentation, con la etiqueta "HTTP/JSON (JWT)".

**Estilo visual:** diagrama plano (flat design), fondo blanco o gris muy claro, líneas
limpias, tipografía sans-serif legible, sin efectos 3D ni fotorrealismo, apto para
imprimir o insertar en una diapositiva. Todo el texto en **español**. Tamaño apaisado
(horizontal, 16:9), buena resolución, texto nítido y suficientemente grande para leerse.

---

## Variante corta (si el prompt anterior es demasiado largo para el modelo)

> Diagrama de arquitectura Onion con 4 anillos concéntricos en colores distintos:
> Domain (centro, azul oscuro: entidades, value objects, servicios de dominio),
> Application (azul medio: casos de uso, DTOs, ports), Infrastructure (naranja: EF Core,
> PostgreSQL, repositorios, JWT), Presentation (verde claro, anillo exterior:
> controllers REST, middleware). Flechas grandes apuntando hacia el centro mostrando
> "Presentation → Application → Domain" e "Infrastructure → Application/Domain vía
> interfaces". Una caja aparte para "Frontend React" conectada por HTTP/JSON al anillo
> Presentation. Estilo diagrama técnico plano, fondo blanco, texto en español, legible,
> formato horizontal 16:9.
