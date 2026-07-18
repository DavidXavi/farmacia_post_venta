using PosFarmacia.Domain.Entities;

namespace PosFarmacia.Domain.Repositories;

public interface IDevolucionRepository : IRepositorio<Devolucion>
{
    Task<IReadOnlyList<Devolucion>> ObtenerPorVentaAsync(Guid ventaId, CancellationToken ct = default);
}
