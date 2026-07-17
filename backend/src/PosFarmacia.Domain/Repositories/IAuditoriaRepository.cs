using PosFarmacia.Domain.Entities;

namespace PosFarmacia.Domain.Repositories;

public interface IAuditoriaRepository : IRepositorio<Auditoria>
{
    Task<IReadOnlyList<Auditoria>> BuscarAsync(DateOnly? fecha, string? entidad, Guid? usuarioId, CancellationToken ct = default);
}
