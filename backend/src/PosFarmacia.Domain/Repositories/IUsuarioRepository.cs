using PosFarmacia.Domain.Entities;

namespace PosFarmacia.Domain.Repositories;

public interface IUsuarioRepository : IRepositorio<Usuario>
{
    Task<Usuario?> ObtenerPorNombreUsuarioAsync(string nombreUsuario, CancellationToken ct = default);
}
