using PosFarmacia.Domain.Entities;

namespace PosFarmacia.Domain.Repositories;

public interface IPromocionRepository : IRepositorio<Promocion>
{
    Task<IReadOnlyList<Promocion>> ObtenerVigentesPorProductoAsync(Guid productoId, DateOnly hoy, CancellationToken ct = default);
}
