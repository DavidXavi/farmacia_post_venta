using Microsoft.EntityFrameworkCore;
using PosFarmacia.Domain.Entities;
using PosFarmacia.Domain.Repositories;
using PosFarmacia.Infrastructure.Persistence;

namespace PosFarmacia.Infrastructure.Repositories;

public sealed class VentaRepository(PosFarmaciaDbContext contexto) : RepositorioBase<Venta>(contexto), IVentaRepository
{
    private IQueryable<Venta> ConAgregado() => Contexto.Ventas
        .Include(v => v.Detalles).ThenInclude(d => d.Lotes)
        .Include(v => v.Pagos)
        .Include(v => v.AplicacionesPromocion)
        .Include(v => v.Comprobante);

    public override async Task<Venta?> ObtenerPorIdAsync(Guid id, CancellationToken ct = default) =>
        await ConAgregado().FirstOrDefaultAsync(v => v.Id == id, ct);

    public async Task<long> ObtenerSiguienteCorrelativoAsync(CancellationToken ct = default)
    {
        var maximo = await Contexto.Ventas.Select(v => (long?)v.NumeroCorrelativo).MaxAsync(ct);
        return (maximo ?? 0) + 1;
    }

    public async Task<IReadOnlyList<Venta>> BuscarAsync(DateOnly? fecha, Guid? localId, Guid? cajaId, Guid? usuarioId, Guid? clienteId, CancellationToken ct = default)
    {
        var query = ConAgregado();

        if (fecha is not null)
        {
            var inicio = DateTime.SpecifyKind(fecha.Value.ToDateTime(TimeOnly.MinValue), DateTimeKind.Utc);
            var fin = inicio.AddDays(1);
            query = query.Where(v => v.Fecha >= inicio && v.Fecha < fin);
        }

        if (cajaId is not null)
        {
            query = query.Where(v => v.CajaId == cajaId);
        }

        if (usuarioId is not null)
        {
            query = query.Where(v => v.UsuarioId == usuarioId);
        }

        if (clienteId is not null)
        {
            query = query.Where(v => v.ClienteId == clienteId);
        }

        var resultado = await query.ToListAsync(ct);

        if (localId is not null)
        {
            var cajasDelLocal = await Contexto.Cajas.Where(c => c.LocalId == localId).Select(c => c.Id).ToListAsync(ct);
            resultado = resultado.Where(v => cajasDelLocal.Contains(v.CajaId)).ToList();
        }

        return resultado;
    }

    public async Task<IReadOnlyList<Venta>> ObtenerPorSesionCajaAsync(Guid sesionCajaId, CancellationToken ct = default) =>
        await ConAgregado().Where(v => v.SesionCajaId == sesionCajaId).ToListAsync(ct);
}

public sealed class DevolucionRepository(PosFarmaciaDbContext contexto) : RepositorioBase<Devolucion>(contexto), IDevolucionRepository
{
    public async Task<IReadOnlyList<Devolucion>> ObtenerPorVentaAsync(Guid ventaId, CancellationToken ct = default) =>
        await Contexto.Devoluciones.Include(d => d.Detalles).Where(d => d.VentaId == ventaId).ToListAsync(ct);
}
