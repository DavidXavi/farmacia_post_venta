using PosFarmacia.Domain.ValueObjects;

namespace PosFarmacia.Domain.Services;

public sealed class CalculadorTotalVenta
{
    public Dinero CalcularSubtotalLinea(Dinero precioUnitario, Cantidad cantidad, Dinero descuento, Porcentaje tasaImpuesto)
    {
        var baseImponible = new Dinero((precioUnitario.Monto * cantidad.Valor) - descuento.Monto);
        var impuesto = new Dinero(tasaImpuesto.AplicarSobre(baseImponible.Monto));
        return baseImponible + impuesto;
    }

    public Dinero CalcularTotalVenta(IEnumerable<Dinero> subtotalesLinea) => new(subtotalesLinea.Sum(s => s.Monto));
}
