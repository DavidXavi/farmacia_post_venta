using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PosFarmacia.Domain.Entities;
using PosFarmacia.Domain.ValueObjects;

namespace PosFarmacia.Infrastructure.Persistence.Configurations;

public sealed class NotaCreditoConfiguration : IEntityTypeConfiguration<NotaCredito>
{
    public void Configure(EntityTypeBuilder<NotaCredito> builder)
    {
        builder.ToTable("notas_credito");
        builder.HasKey(n => n.Id);
        builder.Property(n => n.Motivo).IsRequired().HasMaxLength(500);
        builder.Property(n => n.MontoTotal).IsRequired().HasConversion(m => m.Monto, v => new Dinero(v)).HasColumnType("numeric(12,2)");
    }
}

public sealed class ReglaIncentivoConfiguration : IEntityTypeConfiguration<ReglaIncentivo>
{
    public void Configure(EntityTypeBuilder<ReglaIncentivo> builder)
    {
        builder.ToTable("reglas_incentivo");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Nombre).IsRequired().HasMaxLength(150);
        builder.Property(r => r.MontoPorUnidad).IsRequired().HasConversion(m => m.Monto, v => new Dinero(v)).HasColumnType("numeric(12,2)");

        builder.OwnsOne(r => r.Vigencia, v =>
        {
            v.Property(x => x.Inicio).HasColumnName("vigencia_inicio");
            v.Property(x => x.Fin).HasColumnName("vigencia_fin");
        });
    }
}

public sealed class IncentivoVentaConfiguration : IEntityTypeConfiguration<IncentivoVenta>
{
    public void Configure(EntityTypeBuilder<IncentivoVenta> builder)
    {
        builder.ToTable("incentivos_venta");
        builder.HasKey(i => i.Id);
        builder.Property(i => i.Cantidad).IsRequired().HasConversion(c => c.Valor, v => new Cantidad(v));
        builder.Property(i => i.MontoCalculado).IsRequired().HasConversion(m => m.Monto, v => new Dinero(v)).HasColumnType("numeric(12,2)");
    }
}

public sealed class AuditoriaConfiguration : IEntityTypeConfiguration<Auditoria>
{
    public void Configure(EntityTypeBuilder<Auditoria> builder)
    {
        builder.ToTable("auditoria");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Accion).IsRequired().HasMaxLength(100);
        builder.Property(a => a.Entidad).IsRequired().HasMaxLength(100);
        builder.Property(a => a.EntidadId).IsRequired().HasMaxLength(100);
        builder.Property(a => a.Detalle).HasMaxLength(1000);
    }
}
