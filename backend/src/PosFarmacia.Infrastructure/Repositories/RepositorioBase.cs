using Microsoft.EntityFrameworkCore;
using PosFarmacia.Domain.Entities;
using PosFarmacia.Domain.Repositories;
using PosFarmacia.Infrastructure.Persistence;

namespace PosFarmacia.Infrastructure.Repositories;

public class RepositorioBase<TEntidad>(PosFarmaciaDbContext contexto) : IRepositorio<TEntidad> where TEntidad : Entidad
{
    protected PosFarmaciaDbContext Contexto { get; } = contexto;

    public virtual async Task<TEntidad?> ObtenerPorIdAsync(Guid id, CancellationToken ct = default) =>
        await Contexto.Set<TEntidad>().FirstOrDefaultAsync(e => e.Id == id, ct);

    public virtual async Task<IReadOnlyList<TEntidad>> ObtenerTodosAsync(CancellationToken ct = default) =>
        await Contexto.Set<TEntidad>().ToListAsync(ct);

    public virtual async Task AgregarAsync(TEntidad entidad, CancellationToken ct = default) =>
        await Contexto.Set<TEntidad>().AddAsync(entidad, ct);
}
