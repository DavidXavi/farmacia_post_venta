using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PosFarmacia.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AgregarInventarioYDevoluciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "devoluciones",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    VentaId = table.Column<Guid>(type: "uuid", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    Motivo = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Fecha = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_devoluciones", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "inventarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductoId = table.Column<Guid>(type: "uuid", nullable: false),
                    LocalId = table.Column<Guid>(type: "uuid", nullable: false),
                    CantidadActual = table.Column<int>(type: "integer", nullable: false),
                    ActualizadoEn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inventarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "detalles_devolucion",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DevolucionId = table.Column<Guid>(type: "uuid", nullable: false),
                    DetalleVentaId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductoId = table.Column<Guid>(type: "uuid", nullable: false),
                    Cantidad = table.Column<int>(type: "integer", nullable: false),
                    MontoDevuelto = table.Column<decimal>(type: "numeric(12,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_detalles_devolucion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_detalles_devolucion_devoluciones_DevolucionId",
                        column: x => x.DevolucionId,
                        principalTable: "devoluciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_detalles_devolucion_DevolucionId",
                table: "detalles_devolucion",
                column: "DevolucionId");

            migrationBuilder.CreateIndex(
                name: "IX_inventarios_ProductoId_LocalId",
                table: "inventarios",
                columns: new[] { "ProductoId", "LocalId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "detalles_devolucion");

            migrationBuilder.DropTable(
                name: "inventarios");

            migrationBuilder.DropTable(
                name: "devoluciones");
        }
    }
}
