using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using PosFarmacia.Domain.Entities;

namespace PosFarmacia.Infrastructure.Persistence;

public sealed class PosFarmaciaDbContext(DbContextOptions<PosFarmaciaDbContext> options) : DbContext(options)
{
    public DbSet<Local> Locales => Set<Local>();

    public DbSet<Rol> Roles => Set<Rol>();

    public DbSet<Usuario> Usuarios => Set<Usuario>();

    public DbSet<Caja> Cajas => Set<Caja>();

    public DbSet<SesionCaja> SesionesCaja => Set<SesionCaja>();

    public DbSet<Cliente> Clientes => Set<Cliente>();

    public DbSet<Categoria> Categorias => Set<Categoria>();

    public DbSet<Laboratorio> Laboratorios => Set<Laboratorio>();

    public DbSet<Presentacion> Presentaciones => Set<Presentacion>();

    public DbSet<Producto> Productos => Set<Producto>();

    public DbSet<Lote> Lotes => Set<Lote>();

    public DbSet<MovimientoStock> MovimientosStock => Set<MovimientoStock>();

    public DbSet<Inventario> Inventarios => Set<Inventario>();

    public DbSet<Venta> Ventas => Set<Venta>();

    public DbSet<Promocion> Promociones => Set<Promocion>();

    public DbSet<Receta> Recetas => Set<Receta>();

    public DbSet<ValidacionReceta> ValidacionesReceta => Set<ValidacionReceta>();

    public DbSet<ConvenioSeguro> ConveniosSeguro => Set<ConvenioSeguro>();

    public DbSet<AfiliacionCliente> AfiliacionesCliente => Set<AfiliacionCliente>();

    public DbSet<LineaCredito> LineasCredito => Set<LineaCredito>();

    public DbSet<MovimientoCredito> MovimientosCredito => Set<MovimientoCredito>();

    public DbSet<FormaPago> FormasPago => Set<FormaPago>();

    public DbSet<NotaCredito> NotasCredito => Set<NotaCredito>();

    public DbSet<ReglaIncentivo> ReglasIncentivo => Set<ReglaIncentivo>();

    public DbSet<IncentivoVenta> IncentivosVenta => Set<IncentivoVenta>();

    public DbSet<Auditoria> Auditorias => Set<Auditoria>();

    public DbSet<Devolucion> Devoluciones => Set<Devolucion>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PosFarmaciaDbContext).Assembly);

        // Los Id son GUID v7 generados en el dominio (Entidad.Id), nunca por la base de datos.
        // Sin esto, EF Core asume "clave ya asignada => la fila podria existir" y emite UPDATE en vez de
        // INSERT para entidades hijas nuevas (p.ej. un DetalleVenta agregado a una Venta ya cargada),
        // lo que revienta como una falsa DbUpdateConcurrencyException (0 filas afectadas).
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var idProperty = entityType.FindProperty(nameof(Entidad.Id));
            if (idProperty is not null && idProperty.ClrType == typeof(Guid))
            {
                idProperty.ValueGenerated = ValueGenerated.Never;
            }
        }
    }
}
