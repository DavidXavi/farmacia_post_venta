using PosFarmacia.Domain.Entities;

namespace PosFarmacia.Domain.Repositories;

public interface IRecetaRepository : IRepositorio<Receta>
{
    Task<Receta?> ObtenerPorNumeroAsync(string numero, CancellationToken ct = default);
}
