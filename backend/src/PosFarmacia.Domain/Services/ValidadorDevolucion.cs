using PosFarmacia.Domain.Entities;
using PosFarmacia.Domain.Enums;
using PosFarmacia.Domain.Exceptions;
using PosFarmacia.Domain.ValueObjects;

namespace PosFarmacia.Domain.Services;

public sealed class ValidadorDevolucion
{
    // ponytail: plazo fijo de 30 dias; si la farmacia necesita un plazo configurable por producto/categoria, mover a parametro.
    private const int DiasPlazoDevolucion = 30;

    public void Validar(Venta venta, Producto producto, Cantidad cantidadVendida, Cantidad cantidadYaDevuelta, Cantidad cantidadSolicitada, DateOnly hoy)
    {
        if (venta.Estado != EstadoVenta.Confirmada)
        {
            throw new DevolucionInvalidaException("Solo se puede devolver una venta confirmada.");
        }

        if (producto.EsControlado)
        {
            throw new DevolucionInvalidaException($"El producto {producto.NombreComercial} es controlado y no admite devolucion.");
        }

        var diasTranscurridos = hoy.DayNumber - DateOnly.FromDateTime(venta.Fecha).DayNumber;
        if (diasTranscurridos > DiasPlazoDevolucion)
        {
            throw new DevolucionInvalidaException($"La venta supera el plazo de {DiasPlazoDevolucion} dias permitido para devoluciones.");
        }

        var disponibleParaDevolver = cantidadVendida.Valor - cantidadYaDevuelta.Valor;
        if (cantidadSolicitada.Valor <= 0 || cantidadSolicitada.Valor > disponibleParaDevolver)
        {
            throw new DevolucionInvalidaException("La cantidad a devolver supera lo vendido menos lo ya devuelto en esa linea.");
        }
    }
}
