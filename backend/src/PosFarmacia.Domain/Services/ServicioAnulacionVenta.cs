using PosFarmacia.Domain.Entities;
using PosFarmacia.Domain.ValueObjects;

namespace PosFarmacia.Domain.Services;

public sealed class ServicioAnulacionVenta
{
    public bool RequiereNotaCredito(Venta venta, DateOnly hoy) => !venta.EsDelMismoDia(hoy);

    public IReadOnlyList<(Guid LoteId, Cantidad Cantidad)> ObtenerReversionesDeStock(Venta venta) =>
        venta.Detalles
            .SelectMany(d => d.Lotes.Select(l => (l.LoteId, l.CantidadTomada)))
            .ToList();
}
