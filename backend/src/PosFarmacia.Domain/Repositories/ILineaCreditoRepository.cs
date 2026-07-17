using PosFarmacia.Domain.Entities;

namespace PosFarmacia.Domain.Repositories;

public interface ILineaCreditoRepository : IRepositorio<LineaCredito>
{
    Task<LineaCredito?> ObtenerPorClienteAsync(Guid clienteId, CancellationToken ct = default);
}
