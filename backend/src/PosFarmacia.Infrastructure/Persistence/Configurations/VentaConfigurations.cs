using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PosFarmacia.Domain.Entities;
using PosFarmacia.Domain.ValueObjects;

namespace PosFarmacia.Infrastructure.Persistence.Configurations;

public sealed class VentaConfiguration : IEntityTypeConfiguration<Venta>
{
    public void Configure(EntityTypeBuilder<Venta> builder)
    {
        builder.ToTable("ventas");
        builder.HasKey(v => v.Id);
        builder.Property(v => v.Estado).HasConversion<string>().HasMaxLength(20);
        builder.HasIndex(v => v.NumeroCorrelativo).IsUnique();

        builder.HasMany(v => v.Detalles).WithOne().HasForeignKey(d => d.VentaId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(v => v.Pagos).WithOne().HasForeignKey(p => p.VentaId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(v => v.AplicacionesPromocion).WithOne().HasForeignKey(a => a.VentaId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(v => v.Comprobante).WithOne().HasForeignKey<Comprobante>(c => c.VentaId).OnDelete(DeleteBehavior.Cascade);
    }
}

public sealed class DetalleVentaConfiguration : IEntityTypeConfiguration<DetalleVenta>
{
    public void Configure(EntityTypeBuilder<DetalleVenta> builder)
    {
        builder.ToTable("detalles_venta");
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Cantidad).IsRequired().HasConversion(c => c.Valor, v => new Cantidad(v));
        builder.Property(d => d.PrecioUnitario).IsRequired().HasConversion(m => m.Monto, v => new Dinero(v)).HasColumnType("numeric(12,2)");
        builder.Property(d => d.DescuentoMonto).IsRequired().HasConversion(m => m.Monto, v => new Dinero(v)).HasColumnType("numeric(12,2)");
        builder.Property(d => d.ImpuestoMonto).IsRequired().HasConversion(m => m.Monto, v => new Dinero(v)).HasColumnType("numeric(12,2)");
        builder.Property(d => d.Subtotal).IsRequired().HasConversion(m => m.Monto, v => new Dinero(v)).HasColumnType("numeric(12,2)");

        builder.HasMany(d => d.Lotes).WithOne().HasForeignKey(l => l.DetalleVentaId).OnDelete(DeleteBehavior.Cascade);
    }
}

public sealed class DetalleVentaLoteConfiguration : IEntityTypeConfiguration<DetalleVentaLote>
{
    public void Configure(EntityTypeBuilder<DetalleVentaLote> builder)
    {
        builder.ToTable("detalles_venta_lotes");
        builder.HasKey(d => d.Id);
        builder.Property(d => d.CantidadTomada).IsRequired().HasConversion(c => c.Valor, v => new Cantidad(v));
    }
}

public sealed class AplicacionPromocionConfiguration : IEntityTypeConfiguration<AplicacionPromocion>
{
    public void Configure(EntityTypeBuilder<AplicacionPromocion> builder)
    {
        builder.ToTable("aplicaciones_promocion");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.MontoDescuento).IsRequired().HasConversion(m => m.Monto, v => new Dinero(v)).HasColumnType("numeric(12,2)");
    }
}

public sealed class PagoConfiguration : IEntityTypeConfiguration<Pago>
{
    public void Configure(EntityTypeBuilder<Pago> builder)
    {
        builder.ToTable("pagos");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Monto).IsRequired().HasConversion(m => m.Monto, v => new Dinero(v)).HasColumnType("numeric(12,2)");
        builder.Property(p => p.CodigoAutorizacion).HasMaxLength(100);
    }
}

public sealed class ComprobanteConfiguration : IEntityTypeConfiguration<Comprobante>
{
    public void Configure(EntityTypeBuilder<Comprobante> builder)
    {
        builder.ToTable("comprobantes");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Tipo).HasConversion<string>().HasMaxLength(20);

        builder.OwnsOne(c => c.Numero, n =>
        {
            n.Property(x => x.Serie).HasColumnName("serie").IsRequired().HasMaxLength(10);
            n.Property(x => x.Correlativo).HasColumnName("correlativo").IsRequired();
        });
    }
}
