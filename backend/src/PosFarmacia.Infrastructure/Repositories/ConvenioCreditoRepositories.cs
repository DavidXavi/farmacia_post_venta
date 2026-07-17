using Microsoft.EntityFrameworkCore;
using PosFarmacia.Domain.Entities;
using PosFarmacia.Domain.Repositories;
using PosFarmacia.Infrastructure.Persistence;

namespace PosFarmacia.Infrastructure.Repositories;

public sealed class ConvenioSeguroRepository(PosFarmaciaDbContext contexto) : RepositorioBase<ConvenioSeguro>(contexto), IConvenioSeguroRepository
{
    public override async Task<ConvenioSeguro?> ObtenerPorIdAsync(Guid id, CancellationToken ct = default) =>
        await Contexto.ConveniosSeguro.Include(c => c.Coberturas).FirstOrDefaultAsync(c => c.Id == id, ct);

    public override async Task<IReadOnlyList<ConvenioSeguro>> ObtenerTodosAsync(CancellationToken ct = default) =>
        await Contexto.ConveniosSeguro.Include(c => c.Coberturas).ToListAsync(ct);
}

public sealed class AfiliacionClienteRepository(PosFarmaciaDbContext contexto) : RepositorioBase<AfiliacionCliente>(contexto), IAfiliacionClienteRepository
{
    public async Task<IReadOnlyList<AfiliacionCliente>> ObtenerPorClienteAsync(Guid clienteId, CancellationToken ct = default) =>
        await Contexto.AfiliacionesCliente.Where(a => a.ClienteId == clienteId).ToListAsync(ct);
}

public sealed class LineaCreditoRepository(PosFarmaciaDbContext contexto) : RepositorioBase<LineaCredito>(contexto), ILineaCreditoRepository
{
    public async Task<LineaCredito?> ObtenerPorClienteAsync(Guid clienteId, CancellationToken ct = default) =>
        await Contexto.LineasCredito.FirstOrDefaultAsync(l => l.ClienteId == clienteId, ct);
}

public sealed class MovimientoCreditoRepository(PosFarmaciaDbContext contexto) : RepositorioBase<MovimientoCredito>(contexto), IMovimientoCreditoRepository;
