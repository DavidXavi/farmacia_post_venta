using PosFarmacia.Domain.Entities;

namespace PosFarmacia.Domain.Repositories;

public interface IInventarioRepository : IRepositorio<Inventario>
{
    Task<Inventario?> ObtenerPorProductoYLocalAsync(Guid productoId, Guid localId, CancellationToken ct = default);

    Task<IReadOnlyList<Inventario>> ObtenerPorLocalAsync(Guid localId, CancellationToken ct = default);
}
