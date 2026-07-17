using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PosFarmacia.Domain.Entities;

namespace PosFarmacia.Infrastructure.Persistence.Configurations;

public sealed class LocalConfiguration : IEntityTypeConfiguration<Local>
{
    public void Configure(EntityTypeBuilder<Local> builder)
    {
        builder.ToTable("locales");
        builder.HasKey(l => l.Id);
        builder.Property(l => l.Nombre).IsRequired().HasMaxLength(150);
        builder.Property(l => l.Direccion).IsRequired().HasMaxLength(250);
    }
}

public sealed class RolConfiguration : IEntityTypeConfiguration<Rol>
{
    public void Configure(EntityTypeBuilder<Rol> builder)
    {
        builder.ToTable("roles");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Nombre).IsRequired().HasConversion<string>().HasMaxLength(50);
        builder.HasIndex(r => r.Nombre).IsUnique();
        builder.Property(r => r.Descripcion).HasMaxLength(250);
    }
}

public sealed class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("usuarios");
        builder.HasKey(u => u.Id);
        builder.Property(u => u.NombreUsuario).IsRequired().HasMaxLength(100);
        builder.HasIndex(u => u.NombreUsuario).IsUnique();
        builder.Property(u => u.PasswordHash).IsRequired();
        builder.Property(u => u.Estado).HasConversion<string>().HasMaxLength(20);
        builder.Property(u => u.Permisos).HasConversion<int>();

        builder.HasMany(u => u.Roles)
            .WithOne()
            .HasForeignKey(ur => ur.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public sealed class UsuarioRolConfiguration : IEntityTypeConfiguration<UsuarioRol>
{
    public void Configure(EntityTypeBuilder<UsuarioRol> builder)
    {
        builder.ToTable("usuarios_roles");
        builder.HasKey(ur => new { ur.UsuarioId, ur.RolId });
    }
}

public sealed class CategoriaConfiguration : IEntityTypeConfiguration<Categoria>
{
    public void Configure(EntityTypeBuilder<Categoria> builder)
    {
        builder.ToTable("categorias");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Nombre).IsRequired().HasMaxLength(150);
    }
}

public sealed class LaboratorioConfiguration : IEntityTypeConfiguration<Laboratorio>
{
    public void Configure(EntityTypeBuilder<Laboratorio> builder)
    {
        builder.ToTable("laboratorios");
        builder.HasKey(l => l.Id);
        builder.Property(l => l.Nombre).IsRequired().HasMaxLength(150);
    }
}

public sealed class PresentacionConfiguration : IEntityTypeConfiguration<Presentacion>
{
    public void Configure(EntityTypeBuilder<Presentacion> builder)
    {
        builder.ToTable("presentaciones");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Nombre).IsRequired().HasMaxLength(150);
        builder.Property(p => p.UnidadMedida).IsRequired().HasMaxLength(50);
    }
}

public sealed class FormaPagoConfiguration : IEntityTypeConfiguration<FormaPago>
{
    public void Configure(EntityTypeBuilder<FormaPago> builder)
    {
        builder.ToTable("formas_pago");
        builder.HasKey(f => f.Id);
        builder.Property(f => f.Nombre).IsRequired().HasMaxLength(100);
        builder.Property(f => f.Tipo).HasConversion<string>().HasMaxLength(30);
    }
}

public sealed class CajaConfiguration : IEntityTypeConfiguration<Caja>
{
    public void Configure(EntityTypeBuilder<Caja> builder)
    {
        builder.ToTable("cajas");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Nombre).IsRequired().HasMaxLength(100);
    }
}

public sealed class SesionCajaConfiguration : IEntityTypeConfiguration<SesionCaja>
{
    public void Configure(EntityTypeBuilder<SesionCaja> builder)
    {
        builder.ToTable("sesiones_caja");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Estado).HasConversion<string>().HasMaxLength(20);

        builder.Property(s => s.MontoInicial).HasConversion(m => m.Monto, v => new Domain.ValueObjects.Dinero(v)).HasColumnType("numeric(12,2)");
        builder.Property(s => s.MontoEsperado).HasConversion(m => m == null ? (decimal?)null : m.Monto, v => v == null ? null : new Domain.ValueObjects.Dinero(v.Value)).HasColumnType("numeric(12,2)");
        builder.Property(s => s.MontoDeclarado).HasConversion(m => m == null ? (decimal?)null : m.Monto, v => v == null ? null : new Domain.ValueObjects.Dinero(v.Value)).HasColumnType("numeric(12,2)");
        builder.Property(s => s.Diferencia).HasConversion(m => m == null ? (decimal?)null : m.Monto, v => v == null ? null : new Domain.ValueObjects.Dinero(v.Value)).HasColumnType("numeric(12,2)");
    }
}

public sealed class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
{
    public void Configure(EntityTypeBuilder<Cliente> builder)
    {
        builder.ToTable("clientes");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Dni).IsRequired().HasConversion(d => d.Valor, v => new Domain.ValueObjects.Dni(v)).HasMaxLength(8);
        builder.HasIndex(c => c.Dni).IsUnique();
        builder.Property(c => c.Nombres).IsRequired().HasMaxLength(150);
        builder.Property(c => c.Apellidos).IsRequired().HasMaxLength(150);
        builder.Property(c => c.Estado).HasConversion<string>().HasMaxLength(20);
    }
}
