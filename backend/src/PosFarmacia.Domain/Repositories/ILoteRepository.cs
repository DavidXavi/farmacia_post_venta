using PosFarmacia.Domain.Entities;

namespace PosFarmacia.Domain.Repositories;

public interface ILoteRepository : IRepositorio<Lote>
{
    /// Lotes vendibles del producto en el local, ordenados FEFO (vencimiento mas cercano primero).
    Task<IReadOnlyList<Lote>> ObtenerVendiblesOrdenadosFefoAsync(Guid productoId, Guid localId, DateOnly hoy, CancellationToken ct = default);

    Task<IReadOnlyList<Lote>> ObtenerProximosAVencerAsync(DateOnly hoy, int diasHorizonte, CancellationToken ct = default);
}
