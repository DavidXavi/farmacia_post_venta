using Microsoft.EntityFrameworkCore;
using PosFarmacia.Domain.Entities;
using PosFarmacia.Domain.Repositories;
using PosFarmacia.Infrastructure.Persistence;

namespace PosFarmacia.Infrastructure.Repositories;

public sealed class NotaCreditoRepository(PosFarmaciaDbContext contexto) : RepositorioBase<NotaCredito>(contexto), INotaCreditoRepository;

public sealed class ReglaIncentivoRepository(PosFarmaciaDbContext contexto) : RepositorioBase<ReglaIncentivo>(contexto), IReglaIncentivoRepository;

public sealed class IncentivoVentaRepository(PosFarmaciaDbContext contexto) : RepositorioBase<IncentivoVenta>(contexto), IIncentivoVentaRepository
{
    public async Task<IReadOnlyList<IncentivoVenta>> BuscarAsync(DateOnly desde, DateOnly hasta, Guid? usuarioId, CancellationToken ct = default)
    {
        var inicio = desde.ToDateTime(TimeOnly.MinValue);
        var fin = hasta.ToDateTime(TimeOnly.MaxValue);

        var query = Contexto.IncentivosVenta.Where(i => i.Fecha >= inicio && i.Fecha <= fin);
        if (usuarioId is not null)
        {
            query = query.Where(i => i.UsuarioId == usuarioId);
        }

        return await query.ToListAsync(ct);
    }
}

public sealed class AuditoriaRepository(PosFarmaciaDbContext contexto) : RepositorioBase<Auditoria>(contexto), IAuditoriaRepository
{
    public async Task<IReadOnlyList<Auditoria>> BuscarAsync(DateOnly? fecha, string? entidad, Guid? usuarioId, CancellationToken ct = default)
    {
        var query = Contexto.Auditorias.AsQueryable();

        if (fecha is not null)
        {
            var inicio = fecha.Value.ToDateTime(TimeOnly.MinValue);
            var fin = inicio.AddDays(1);
            query = query.Where(a => a.Fecha >= inicio && a.Fecha < fin);
        }

        if (!string.IsNullOrWhiteSpace(entidad))
        {
            query = query.Where(a => a.Entidad == entidad);
        }

        if (usuarioId is not null)
        {
            query = query.Where(a => a.UsuarioId == usuarioId);
        }

        return await query.OrderByDescending(a => a.Fecha).ToListAsync(ct);
    }
}
