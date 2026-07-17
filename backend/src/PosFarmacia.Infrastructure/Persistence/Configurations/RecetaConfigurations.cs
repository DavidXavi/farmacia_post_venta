using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PosFarmacia.Domain.Entities;
using PosFarmacia.Domain.ValueObjects;

namespace PosFarmacia.Infrastructure.Persistence.Configurations;

public sealed class RecetaConfiguration : IEntityTypeConfiguration<Receta>
{
    public void Configure(EntityTypeBuilder<Receta> builder)
    {
        builder.ToTable("recetas");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Numero).IsRequired().HasConversion(n => n.Valor, v => new NumeroReceta(v)).HasMaxLength(50);
        builder.HasIndex(r => r.Numero).IsUnique();
        builder.Property(r => r.Tipo).HasConversion<string>().HasMaxLength(30);
        builder.Property(r => r.Estado).HasConversion<string>().HasMaxLength(20);
        builder.Property(r => r.DatosPaciente).IsRequired().HasMaxLength(300);
        builder.Property(r => r.DatosProfesional).IsRequired().HasMaxLength(300);
        builder.Property(r => r.DosisYCantidadAutorizada).IsRequired().HasMaxLength(300);
        builder.Property(r => r.ArchivoRespaldoUrl).HasMaxLength(500);
    }
}

public sealed class ValidacionRecetaConfiguration : IEntityTypeConfiguration<ValidacionReceta>
{
    public void Configure(EntityTypeBuilder<ValidacionReceta> builder)
    {
        builder.ToTable("validaciones_receta");
        builder.HasKey(v => v.Id);
        builder.Property(v => v.Resultado).HasConversion<string>().HasMaxLength(20);
        builder.Property(v => v.Observaciones).HasMaxLength(500);
    }
}
