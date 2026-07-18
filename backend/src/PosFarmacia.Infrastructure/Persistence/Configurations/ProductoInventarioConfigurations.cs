using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PosFarmacia.Domain.Entities;
using PosFarmacia.Domain.ValueObjects;

namespace PosFarmacia.Infrastructure.Persistence.Configurations;

public sealed class ProductoConfiguration : IEntityTypeConfiguration<Producto>
{
    public void Configure(EntityTypeBuilder<Producto> builder)
    {
        builder.ToTable("productos");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.CodigoInterno).IsRequired().HasConversion(c => c.Valor, v => new CodigoProducto(v)).HasMaxLength(50);
        builder.HasIndex(p => p.CodigoInterno).IsUnique();

        builder.Property(p => p.CodigoBarras).HasConversion(
            c => c == null ? null : c.Valor,
            v => v == null ? null : new CodigoBarras(v)).HasMaxLength(50);

        builder.Property(p => p.NombreComercial).IsRequired().HasMaxLength(200);
        builder.Property(p => p.Descripcion).HasMaxLength(500);
        builder.Property(p => p.TipoProducto).HasConversion<string>().HasMaxLength(30);
        builder.Property(p => p.PrecioVenta).IsRequired().HasConversion(m => m.Monto, v => new Dinero(v)).HasColumnType("numeric(12,2)");
        builder.Property(p => p.TipoRecetaRequerida).HasConversion<string>().HasMaxLength(30);
        builder.Property(p => p.Estado).HasConversion<string>().HasMaxLength(20);
    }
}

public sealed class LoteConfiguration : IEntityTypeConfiguration<Lote>
{
    public void Configure(EntityTypeBuilder<Lote> builder)
    {
        builder.ToTable("lotes");
        builder.HasKey(l => l.Id);

        builder.Property(l => l.Codigo).IsRequired().HasConversion(c => c.Valor, v => new CodigoLote(v)).HasMaxLength(50);
        builder.Property(l => l.FechaVencimiento).IsRequired().HasConversion(f => f.Valor, v => new FechaVencimiento(v)).HasColumnName("fecha_vencimiento");
        builder.Property(l => l.CantidadRecibida).IsRequired().HasConversion(c => c.Valor, v => new Cantidad(v));
        builder.Property(l => l.CantidadDisponible).IsRequired().HasConversion(c => c.Valor, v => new Cantidad(v));
        builder.Property(l => l.Costo).HasConversion(
            m => m == null ? (decimal?)null : m.Monto,
            v => v == null ? null : new Dinero(v.Value)).HasColumnType("numeric(12,2)");
        builder.Property(l => l.Estado).HasConversion<string>().HasMaxLength(20);

        builder.HasIndex(l => new { l.ProductoId, l.LocalId, l.FechaVencimiento });

        // RN02/concurrencia: si dos cajeros confirman a la vez sobre el mismo lote, el segundo debe fallar con 409
        // en lugar de sobrescribir el stock. xmin es la columna de version nativa de Postgres (sin columna extra).
        builder.Property<uint>("xmin").HasColumnName("xmin").HasColumnType("xid").ValueGeneratedOnAddOrUpdate().IsConcurrencyToken();
    }
}

public sealed class MovimientoStockConfiguration : IEntityTypeConfiguration<MovimientoStock>
{
    public void Configure(EntityTypeBuilder<MovimientoStock> builder)
    {
        builder.ToTable("movimientos_stock");
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Tipo).HasConversion<string>().HasMaxLength(30);
        builder.Property(m => m.Cantidad).IsRequired().HasConversion(c => c.Valor, v => new Cantidad(v));
        builder.Property(m => m.Referencia).HasMaxLength(100);
    }
}

public sealed class InventarioConfiguration : IEntityTypeConfiguration<Inventario>
{
    public void Configure(EntityTypeBuilder<Inventario> builder)
    {
        builder.ToTable("inventarios");
        builder.HasKey(i => i.Id);
        builder.Property(i => i.CantidadActual).IsRequired().HasConversion(c => c.Valor, v => new Cantidad(v));

        builder.HasIndex(i => new { i.ProductoId, i.LocalId }).IsUnique();
    }
}
