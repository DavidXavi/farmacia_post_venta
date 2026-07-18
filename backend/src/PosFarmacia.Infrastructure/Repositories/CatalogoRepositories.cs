using Microsoft.EntityFrameworkCore;
using PosFarmacia.Domain.Entities;
using PosFarmacia.Domain.Enums;
using PosFarmacia.Domain.Repositories;
using PosFarmacia.Infrastructure.Persistence;

namespace PosFarmacia.Infrastructure.Repositories;

public sealed class LocalRepository(PosFarmaciaDbContext contexto) : RepositorioBase<Local>(contexto), ILocalRepository;

public sealed class RolRepository(PosFarmaciaDbContext contexto) : RepositorioBase<Rol>(contexto), IRolRepository
{
    public async Task<Rol?> ObtenerPorNombreAsync(RolNombre nombre, CancellationToken ct = default) =>
        await Contexto.Roles.FirstOrDefaultAsync(r => r.Nombre == nombre, ct);
}

public sealed class UsuarioRepository(PosFarmaciaDbContext contexto) : RepositorioBase<Usuario>(contexto), IUsuarioRepository
{
    public override async Task<Usuario?> ObtenerPorIdAsync(Guid id, CancellationToken ct = default) =>
        await Contexto.Usuarios.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Id == id, ct);

    public override async Task<IReadOnlyList<Usuario>> ObtenerTodosAsync(CancellationToken ct = default) =>
        await Contexto.Usuarios.Include(u => u.Roles).ToListAsync(ct);

    public async Task<Usuario?> ObtenerPorNombreUsuarioAsync(string nombreUsuario, CancellationToken ct = default) =>
        await Contexto.Usuarios.Include(u => u.Roles).FirstOrDefaultAsync(u => u.NombreUsuario == nombreUsuario, ct);
}

public sealed class CajaRepository(PosFarmaciaDbContext contexto) : RepositorioBase<Caja>(contexto), ICajaRepository;

public sealed class SesionCajaRepository(PosFarmaciaDbContext contexto) : RepositorioBase<SesionCaja>(contexto), ISesionCajaRepository
{
    public async Task<SesionCaja?> ObtenerSesionActivaAsync(Guid cajaId, CancellationToken ct = default) =>
        await Contexto.SesionesCaja.FirstOrDefaultAsync(s => s.CajaId == cajaId && s.Estado == EstadoCaja.Abierta, ct);
}

public sealed class ClienteRepository(PosFarmaciaDbContext contexto) : RepositorioBase<Cliente>(contexto), IClienteRepository
{
    public async Task<Cliente?> ObtenerPorDniAsync(string dni, CancellationToken ct = default) =>
        await Contexto.Clientes.FirstOrDefaultAsync(c => c.Dni == new Domain.ValueObjects.Dni(dni), ct);
}

public sealed class CategoriaRepository(PosFarmaciaDbContext contexto) : RepositorioBase<Categoria>(contexto), ICategoriaRepository;

public sealed class LaboratorioRepository(PosFarmaciaDbContext contexto) : RepositorioBase<Laboratorio>(contexto), ILaboratorioRepository;

public sealed class PresentacionRepository(PosFarmaciaDbContext contexto) : RepositorioBase<Presentacion>(contexto), IPresentacionRepository;

public sealed class FormaPagoRepository(PosFarmaciaDbContext contexto) : RepositorioBase<FormaPago>(contexto), IFormaPagoRepository;
