using PosFarmacia.Domain.Entities;
using PosFarmacia.Domain.Exceptions;
using PosFarmacia.Domain.ValueObjects;

namespace PosFarmacia.Domain.Services;

public sealed class AsignadorLotesFEFO
{
    /// Los lotes deben venir ya filtrados como vendibles y ordenados por vencimiento ascendente.
    public IReadOnlyList<(Guid LoteId, Cantidad Cantidad)> Asignar(IReadOnlyList<Lote> lotesVendiblesOrdenados, Cantidad cantidadRequerida)
    {
        var asignaciones = new List<(Guid LoteId, Cantidad Cantidad)>();
        var restante = cantidadRequerida.Valor;

        foreach (var lote in lotesVendiblesOrdenados)
        {
            if (restante <= 0)
            {
                break;
            }

            var tomar = Math.Min(restante, lote.CantidadDisponible.Valor);
            if (tomar <= 0)
            {
                continue;
            }

            asignaciones.Add((lote.Id, new Cantidad(tomar)));
            restante -= tomar;
        }

        if (restante > 0)
        {
            throw new StockInsuficienteException("No hay stock vendible suficiente entre los lotes disponibles para completar la cantidad solicitada.");
        }

        return asignaciones;
    }
}
