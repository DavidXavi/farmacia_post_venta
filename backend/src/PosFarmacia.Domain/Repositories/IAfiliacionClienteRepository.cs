using PosFarmacia.Domain.Entities;

namespace PosFarmacia.Domain.Repositories;

public interface IAfiliacionClienteRepository : IRepositorio<AfiliacionCliente>
{
    Task<IReadOnlyList<AfiliacionCliente>> ObtenerPorClienteAsync(Guid clienteId, CancellationToken ct = default);
}
