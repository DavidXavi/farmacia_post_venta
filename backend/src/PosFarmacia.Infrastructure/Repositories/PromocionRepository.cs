using Microsoft.EntityFrameworkCore;
using PosFarmacia.Domain.Entities;
using PosFarmacia.Domain.Repositories;
using PosFarmacia.Infrastructure.Persistence;

namespace PosFarmacia.Infrastructure.Repositories;

public sealed class PromocionRepository(PosFarmaciaDbContext contexto) : RepositorioBase<Promocion>(contexto), IPromocionRepository
{
    public override async Task<Promocion?> ObtenerPorIdAsync(Guid id, CancellationToken ct = default) =>
        await Contexto.Promociones.Include(p => p.Productos).FirstOrDefaultAsync(p => p.Id == id, ct);

    public override async Task<IReadOnlyList<Promocion>> ObtenerTodosAsync(CancellationToken ct = default) =>
        await Contexto.Promociones.Include(p => p.Productos).ToListAsync(ct);

    public async Task<IReadOnlyList<Promocion>> ObtenerVigentesPorProductoAsync(Guid productoId, DateOnly hoy, CancellationToken ct = default)
    {
        return await Contexto.Promociones
            .Include(p => p.Productos)
            .Where(p => p.Activa)
            .Where(p => (p.Vigencia.Inicio == null || p.Vigencia.Inicio <= hoy) && (p.Vigencia.Fin == null || p.Vigencia.Fin >= hoy))
            .Where(p => p.Productos.Any(pp => pp.ProductoId == productoId))
            .ToListAsync(ct);
    }
}
