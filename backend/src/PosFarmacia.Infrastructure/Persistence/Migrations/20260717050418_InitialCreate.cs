using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PosFarmacia.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "afiliaciones_cliente",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ClienteId = table.Column<Guid>(type: "uuid", nullable: false),
                    ConvenioId = table.Column<Guid>(type: "uuid", nullable: false),
                    vigencia_inicio = table.Column<DateOnly>(type: "date", nullable: true),
                    vigencia_fin = table.Column<DateOnly>(type: "date", nullable: true),
                    Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_afiliaciones_cliente", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "auditoria",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Fecha = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    Accion = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Entidad = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    EntidadId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Detalle = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    DatosAnteriores = table.Column<string>(type: "text", nullable: true),
                    DatosNuevos = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_auditoria", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "cajas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LocalId = table.Column<Guid>(type: "uuid", nullable: false),
                    Activa = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cajas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "categorias",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categorias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "clientes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Dni = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    Nombres = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Apellidos = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    FechaNacimiento = table.Column<DateOnly>(type: "date", nullable: true),
                    Telefono = table.Column<string>(type: "text", nullable: true),
                    Correo = table.Column<string>(type: "text", nullable: true),
                    Direccion = table.Column<string>(type: "text", nullable: true),
                    Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clientes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "convenios_seguro",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Activo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_convenios_seguro", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "formas_pago",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Tipo = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Activo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_formas_pago", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "incentivos_venta",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ReglaIncentivoId = table.Column<Guid>(type: "uuid", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    VentaId = table.Column<Guid>(type: "uuid", nullable: false),
                    DetalleVentaId = table.Column<Guid>(type: "uuid", nullable: false),
                    Cantidad = table.Column<int>(type: "integer", nullable: false),
                    MontoCalculado = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    Fecha = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_incentivos_venta", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "laboratorios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_laboratorios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "lineas_credito",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ClienteId = table.Column<Guid>(type: "uuid", nullable: false),
                    MontoAutorizado = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    SaldoDisponible = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    vigencia_inicio = table.Column<DateOnly>(type: "date", nullable: true),
                    vigencia_fin = table.Column<DateOnly>(type: "date", nullable: true),
                    Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lineas_credito", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "locales",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Direccion = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Activo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_locales", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "lotes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Codigo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ProductoId = table.Column<Guid>(type: "uuid", nullable: false),
                    fecha_vencimiento = table.Column<DateOnly>(type: "date", nullable: false),
                    CantidadRecibida = table.Column<int>(type: "integer", nullable: false),
                    CantidadDisponible = table.Column<int>(type: "integer", nullable: false),
                    Costo = table.Column<decimal>(type: "numeric(12,2)", nullable: true),
                    LocalId = table.Column<Guid>(type: "uuid", nullable: false),
                    Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lotes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "movimientos_credito",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LineaCreditoId = table.Column<Guid>(type: "uuid", nullable: false),
                    VentaId = table.Column<Guid>(type: "uuid", nullable: true),
                    Tipo = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Monto = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    Fecha = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_movimientos_credito", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "movimientos_stock",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LoteId = table.Column<Guid>(type: "uuid", nullable: false),
                    Tipo = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Cantidad = table.Column<int>(type: "integer", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    Referencia = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Fecha = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_movimientos_stock", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "notas_credito",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    VentaId = table.Column<Guid>(type: "uuid", nullable: false),
                    ComprobanteId = table.Column<Guid>(type: "uuid", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    Motivo = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    MontoTotal = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    Fecha = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notas_credito", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "presentaciones",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    UnidadMedida = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_presentaciones", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "productos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CodigoInterno = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CodigoBarras = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    NombreComercial = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    TipoProducto = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    CategoriaId = table.Column<Guid>(type: "uuid", nullable: false),
                    LaboratorioId = table.Column<Guid>(type: "uuid", nullable: false),
                    PresentacionId = table.Column<Guid>(type: "uuid", nullable: false),
                    PrecioVenta = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    EsControlado = table.Column<bool>(type: "boolean", nullable: false),
                    RequiereReceta = table.Column<bool>(type: "boolean", nullable: false),
                    TipoRecetaRequerida = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "promociones",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    TipoBeneficio = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    ValorBeneficio = table.Column<decimal>(type: "numeric", nullable: false),
                    RequiereCliente = table.Column<bool>(type: "boolean", nullable: false),
                    CantidadMinima = table.Column<int>(type: "integer", nullable: false),
                    vigencia_inicio = table.Column<DateOnly>(type: "date", nullable: true),
                    vigencia_fin = table.Column<DateOnly>(type: "date", nullable: true),
                    Activa = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_promociones", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "recetas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Numero = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Tipo = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    FechaEmision = table.Column<DateOnly>(type: "date", nullable: false),
                    FechaVencimiento = table.Column<DateOnly>(type: "date", nullable: true),
                    ProductoId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClienteId = table.Column<Guid>(type: "uuid", nullable: true),
                    DatosPaciente = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    DatosProfesional = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    DosisYCantidadAutorizada = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    ArchivoRespaldoUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    RetenidaEnBotica = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_recetas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "reglas_incentivo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    ProductoId = table.Column<Guid>(type: "uuid", nullable: true),
                    CategoriaId = table.Column<Guid>(type: "uuid", nullable: true),
                    MontoPorUnidad = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    vigencia_inicio = table.Column<DateOnly>(type: "date", nullable: true),
                    vigencia_fin = table.Column<DateOnly>(type: "date", nullable: true),
                    Activa = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reglas_incentivo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "sesiones_caja",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CajaId = table.Column<Guid>(type: "uuid", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    FechaApertura = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MontoInicial = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    FechaCierre = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    MontoEsperado = table.Column<decimal>(type: "numeric(12,2)", nullable: true),
                    MontoDeclarado = table.Column<decimal>(type: "numeric(12,2)", nullable: true),
                    Diferencia = table.Column<decimal>(type: "numeric(12,2)", nullable: true),
                    ObservacionCierre = table.Column<string>(type: "text", nullable: true),
                    Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sesiones_caja", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "usuarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    NombreUsuario = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    LocalId = table.Column<Guid>(type: "uuid", nullable: false),
                    Permisos = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "validaciones_receta",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RecetaId = table.Column<Guid>(type: "uuid", nullable: false),
                    UsuarioValidadorId = table.Column<Guid>(type: "uuid", nullable: false),
                    Fecha = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Resultado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Observaciones = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_validaciones_receta", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ventas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    NumeroCorrelativo = table.Column<long>(type: "bigint", nullable: true),
                    Fecha = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CajaId = table.Column<Guid>(type: "uuid", nullable: false),
                    SesionCajaId = table.Column<Guid>(type: "uuid", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClienteId = table.Column<Guid>(type: "uuid", nullable: true),
                    ConvenioSeguroId = table.Column<Guid>(type: "uuid", nullable: true),
                    LineaCreditoId = table.Column<Guid>(type: "uuid", nullable: true),
                    Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ventas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "coberturas_convenio",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ConvenioId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductoId = table.Column<Guid>(type: "uuid", nullable: false),
                    PorcentajeCubierto = table.Column<decimal>(type: "numeric(5,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_coberturas_convenio", x => x.Id);
                    table.ForeignKey(
                        name: "FK_coberturas_convenio_convenios_seguro_ConvenioId",
                        column: x => x.ConvenioId,
                        principalTable: "convenios_seguro",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "promociones_productos",
                columns: table => new
                {
                    PromocionId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductoId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_promociones_productos", x => new { x.PromocionId, x.ProductoId });
                    table.ForeignKey(
                        name: "FK_promociones_productos_promociones_PromocionId",
                        column: x => x.PromocionId,
                        principalTable: "promociones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "usuarios_roles",
                columns: table => new
                {
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    RolId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuarios_roles", x => new { x.UsuarioId, x.RolId });
                    table.ForeignKey(
                        name: "FK_usuarios_roles_usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "aplicaciones_promocion",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    VentaId = table.Column<Guid>(type: "uuid", nullable: false),
                    DetalleVentaId = table.Column<Guid>(type: "uuid", nullable: false),
                    PromocionId = table.Column<Guid>(type: "uuid", nullable: false),
                    MontoDescuento = table.Column<decimal>(type: "numeric(12,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_aplicaciones_promocion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_aplicaciones_promocion_ventas_VentaId",
                        column: x => x.VentaId,
                        principalTable: "ventas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "comprobantes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    VentaId = table.Column<Guid>(type: "uuid", nullable: false),
                    Tipo = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    serie = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    correlativo = table.Column<int>(type: "integer", nullable: false),
                    FechaEmision = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_comprobantes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_comprobantes_ventas_VentaId",
                        column: x => x.VentaId,
                        principalTable: "ventas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "detalles_venta",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    VentaId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductoId = table.Column<Guid>(type: "uuid", nullable: false),
                    Cantidad = table.Column<int>(type: "integer", nullable: false),
                    PrecioUnitario = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    PromocionAplicadaId = table.Column<Guid>(type: "uuid", nullable: true),
                    RecetaId = table.Column<Guid>(type: "uuid", nullable: true),
                    DescuentoMonto = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    ImpuestoMonto = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    Subtotal = table.Column<decimal>(type: "numeric(12,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_detalles_venta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_detalles_venta_ventas_VentaId",
                        column: x => x.VentaId,
                        principalTable: "ventas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "pagos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    VentaId = table.Column<Guid>(type: "uuid", nullable: false),
                    FormaPagoId = table.Column<Guid>(type: "uuid", nullable: false),
                    Monto = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    CodigoAutorizacion = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Fecha = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pagos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_pagos_ventas_VentaId",
                        column: x => x.VentaId,
                        principalTable: "ventas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "detalles_venta_lotes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DetalleVentaId = table.Column<Guid>(type: "uuid", nullable: false),
                    LoteId = table.Column<Guid>(type: "uuid", nullable: false),
                    CantidadTomada = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_detalles_venta_lotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_detalles_venta_lotes_detalles_venta_DetalleVentaId",
                        column: x => x.DetalleVentaId,
                        principalTable: "detalles_venta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_aplicaciones_promocion_VentaId",
                table: "aplicaciones_promocion",
                column: "VentaId");

            migrationBuilder.CreateIndex(
                name: "IX_clientes_Dni",
                table: "clientes",
                column: "Dni",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_coberturas_convenio_ConvenioId_ProductoId",
                table: "coberturas_convenio",
                columns: new[] { "ConvenioId", "ProductoId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_comprobantes_VentaId",
                table: "comprobantes",
                column: "VentaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_detalles_venta_VentaId",
                table: "detalles_venta",
                column: "VentaId");

            migrationBuilder.CreateIndex(
                name: "IX_detalles_venta_lotes_DetalleVentaId",
                table: "detalles_venta_lotes",
                column: "DetalleVentaId");

            migrationBuilder.CreateIndex(
                name: "IX_lotes_ProductoId_LocalId_fecha_vencimiento",
                table: "lotes",
                columns: new[] { "ProductoId", "LocalId", "fecha_vencimiento" });

            migrationBuilder.CreateIndex(
                name: "IX_pagos_VentaId",
                table: "pagos",
                column: "VentaId");

            migrationBuilder.CreateIndex(
                name: "IX_productos_CodigoInterno",
                table: "productos",
                column: "CodigoInterno",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_recetas_Numero",
                table: "recetas",
                column: "Numero",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_roles_Nombre",
                table: "roles",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_usuarios_NombreUsuario",
                table: "usuarios",
                column: "NombreUsuario",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ventas_NumeroCorrelativo",
                table: "ventas",
                column: "NumeroCorrelativo",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "afiliaciones_cliente");

            migrationBuilder.DropTable(
                name: "aplicaciones_promocion");

            migrationBuilder.DropTable(
                name: "auditoria");

            migrationBuilder.DropTable(
                name: "cajas");

            migrationBuilder.DropTable(
                name: "categorias");

            migrationBuilder.DropTable(
                name: "clientes");

            migrationBuilder.DropTable(
                name: "coberturas_convenio");

            migrationBuilder.DropTable(
                name: "comprobantes");

            migrationBuilder.DropTable(
                name: "detalles_venta_lotes");

            migrationBuilder.DropTable(
                name: "formas_pago");

            migrationBuilder.DropTable(
                name: "incentivos_venta");

            migrationBuilder.DropTable(
                name: "laboratorios");

            migrationBuilder.DropTable(
                name: "lineas_credito");

            migrationBuilder.DropTable(
                name: "locales");

            migrationBuilder.DropTable(
                name: "lotes");

            migrationBuilder.DropTable(
                name: "movimientos_credito");

            migrationBuilder.DropTable(
                name: "movimientos_stock");

            migrationBuilder.DropTable(
                name: "notas_credito");

            migrationBuilder.DropTable(
                name: "pagos");

            migrationBuilder.DropTable(
                name: "presentaciones");

            migrationBuilder.DropTable(
                name: "productos");

            migrationBuilder.DropTable(
                name: "promociones_productos");

            migrationBuilder.DropTable(
                name: "recetas");

            migrationBuilder.DropTable(
                name: "reglas_incentivo");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "sesiones_caja");

            migrationBuilder.DropTable(
                name: "usuarios_roles");

            migrationBuilder.DropTable(
                name: "validaciones_receta");

            migrationBuilder.DropTable(
                name: "convenios_seguro");

            migrationBuilder.DropTable(
                name: "detalles_venta");

            migrationBuilder.DropTable(
                name: "promociones");

            migrationBuilder.DropTable(
                name: "usuarios");

            migrationBuilder.DropTable(
                name: "ventas");
        }
    }
}
