using PosFarmacia.Domain.Entities;

namespace PosFarmacia.Domain.Repositories;

public interface IIncentivoVentaRepository : IRepositorio<IncentivoVenta>
{
    Task<IReadOnlyList<IncentivoVenta>> BuscarAsync(DateOnly desde, DateOnly hasta, Guid? usuarioId, CancellationToken ct = default);
}
