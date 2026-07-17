using PosFarmacia.Domain.Repositories;
using PosFarmacia.Infrastructure.Persistence;

namespace PosFarmacia.Infrastructure.Repositories;

public sealed class UnitOfWork(PosFarmaciaDbContext contexto) : IUnitOfWork
{
    public Task GuardarCambiosAsync(CancellationToken ct = default) => contexto.SaveChangesAsync(ct);
}
