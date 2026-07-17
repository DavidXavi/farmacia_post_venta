using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PosFarmacia.Domain.Entities;
using PosFarmacia.Domain.ValueObjects;

namespace PosFarmacia.Infrastructure.Persistence.Configurations;

public sealed class PromocionConfiguration : IEntityTypeConfiguration<Promocion>
{
    public void Configure(EntityTypeBuilder<Promocion> builder)
    {
        builder.ToTable("promociones");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Nombre).IsRequired().HasMaxLength(150);
        builder.Property(p => p.Descripcion).HasMaxLength(500);
        builder.Property(p => p.TipoBeneficio).HasConversion<string>().HasMaxLength(30);
        builder.Property(p => p.CantidadMinima).IsRequired().HasConversion(c => c.Valor, v => new Cantidad(v));

        builder.OwnsOne(p => p.Vigencia, v =>
        {
            v.Property(x => x.Inicio).HasColumnName("vigencia_inicio");
            v.Property(x => x.Fin).HasColumnName("vigencia_fin");
        });

        builder.HasMany(p => p.Productos).WithOne().HasForeignKey(pp => pp.PromocionId).OnDelete(DeleteBehavior.Cascade);
    }
}

public sealed class PromocionProductoConfiguration : IEntityTypeConfiguration<PromocionProducto>
{
    public void Configure(EntityTypeBuilder<PromocionProducto> builder)
    {
        builder.ToTable("promociones_productos");
        builder.HasKey(pp => new { pp.PromocionId, pp.ProductoId });
    }
}
