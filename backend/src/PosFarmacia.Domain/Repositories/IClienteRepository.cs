using PosFarmacia.Domain.Entities;

namespace PosFarmacia.Domain.Repositories;

public interface IClienteRepository : IRepositorio<Cliente>
{
    Task<Cliente?> ObtenerPorDniAsync(string dni, CancellationToken ct = default);
}
