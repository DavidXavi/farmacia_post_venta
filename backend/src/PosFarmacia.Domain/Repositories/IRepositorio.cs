using PosFarmacia.Domain.Entities;

namespace PosFarmacia.Domain.Repositories;

public interface IRepositorio<TEntidad> where TEntidad : Entidad
{
    Task<TEntidad?> ObtenerPorIdAsync(Guid id, CancellationToken ct = default);

    Task<IReadOnlyList<TEntidad>> ObtenerTodosAsync(CancellationToken ct = default);

    Task AgregarAsync(TEntidad entidad, CancellationToken ct = default);
}

public interface IUnitOfWork
{
    Task GuardarCambiosAsync(CancellationToken ct = default);
}
