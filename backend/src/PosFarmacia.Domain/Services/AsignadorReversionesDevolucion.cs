using PosFarmacia.Domain.Entities;
using PosFarmacia.Domain.Exceptions;
using PosFarmacia.Domain.ValueObjects;

namespace PosFarmacia.Domain.Services;

/// Distribuye la cantidad devuelta entre los mismos lotes que surtieron originalmente la linea de venta.
public sealed class AsignadorReversionesDevolucion
{
    public IReadOnlyList<(Guid LoteId, Cantidad Cantidad)> Asignar(DetalleVenta detalle, Cantidad cantidadADevolver)
    {
        var restante = cantidadADevolver.Valor;
        var resultado = new List<(Guid, Cantidad)>();

        foreach (var loteAsignado in detalle.Lotes)
        {
            if (restante <= 0)
            {
                break;
            }

            var tomar = Math.Min(restante, loteAsignado.CantidadTomada.Valor);
            resultado.Add((loteAsignado.LoteId, new Cantidad(tomar)));
            restante -= tomar;
        }

        if (restante > 0)
        {
            throw new DevolucionInvalidaException("No se pudo distribuir la cantidad a devolver entre los lotes originales de la venta.");
        }

        return resultado;
    }
}
