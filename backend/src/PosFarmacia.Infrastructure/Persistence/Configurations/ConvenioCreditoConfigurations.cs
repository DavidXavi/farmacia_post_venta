using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PosFarmacia.Domain.Entities;
using PosFarmacia.Domain.ValueObjects;

namespace PosFarmacia.Infrastructure.Persistence.Configurations;

public sealed class ConvenioSeguroConfiguration : IEntityTypeConfiguration<ConvenioSeguro>
{
    public void Configure(EntityTypeBuilder<ConvenioSeguro> builder)
    {
        builder.ToTable("convenios_seguro");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Nombre).IsRequired().HasMaxLength(150);

        builder.HasMany(c => c.Coberturas).WithOne().HasForeignKey(cc => cc.ConvenioId).OnDelete(DeleteBehavior.Cascade);
    }
}

public sealed class CoberturaConvenioConfiguration : IEntityTypeConfiguration<CoberturaConvenio>
{
    public void Configure(EntityTypeBuilder<CoberturaConvenio> builder)
    {
        builder.ToTable("coberturas_convenio");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.PorcentajeCubierto).IsRequired().HasConversion(p => p.Valor, v => new Porcentaje(v)).HasColumnType("numeric(5,2)");
        builder.HasIndex(c => new { c.ConvenioId, c.ProductoId }).IsUnique();
    }
}

public sealed class AfiliacionClienteConfiguration : IEntityTypeConfiguration<AfiliacionCliente>
{
    public void Configure(EntityTypeBuilder<AfiliacionCliente> builder)
    {
        builder.ToTable("afiliaciones_cliente");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Estado).HasConversion<string>().HasMaxLength(20);

        builder.OwnsOne(a => a.Vigencia, v =>
        {
            v.Property(x => x.Inicio).HasColumnName("vigencia_inicio");
            v.Property(x => x.Fin).HasColumnName("vigencia_fin");
        });
    }
}

public sealed class LineaCreditoConfiguration : IEntityTypeConfiguration<LineaCredito>
{
    public void Configure(EntityTypeBuilder<LineaCredito> builder)
    {
        builder.ToTable("lineas_credito");
        builder.HasKey(l => l.Id);
        builder.Property(l => l.Estado).HasConversion<string>().HasMaxLength(20);
        builder.Property(l => l.MontoAutorizado).IsRequired().HasConversion(m => m.Monto, v => new Dinero(v)).HasColumnType("numeric(12,2)");
        builder.Property(l => l.SaldoDisponible).IsRequired().HasConversion(m => m.Monto, v => new Dinero(v)).HasColumnType("numeric(12,2)");

        builder.OwnsOne(l => l.Vigencia, v =>
        {
            v.Property(x => x.Inicio).HasColumnName("vigencia_inicio");
            v.Property(x => x.Fin).HasColumnName("vigencia_fin");
        });
    }
}

public sealed class MovimientoCreditoConfiguration : IEntityTypeConfiguration<MovimientoCredito>
{
    public void Configure(EntityTypeBuilder<MovimientoCredito> builder)
    {
        builder.ToTable("movimientos_credito");
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Tipo).HasConversion<string>().HasMaxLength(20);
        builder.Property(m => m.Monto).IsRequired().HasConversion(mo => mo.Monto, v => new Dinero(v)).HasColumnType("numeric(12,2)");
    }
}
