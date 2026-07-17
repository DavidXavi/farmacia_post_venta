using PosFarmacia.Domain.Entities;
using PosFarmacia.Domain.Enums;

namespace PosFarmacia.Domain.Repositories;

public interface IRolRepository : IRepositorio<Rol>
{
    Task<Rol?> ObtenerPorNombreAsync(RolNombre nombre, CancellationToken ct = default);
}
