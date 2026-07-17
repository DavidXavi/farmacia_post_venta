using PosFarmacia.Domain.Entities;
using PosFarmacia.Domain.ValueObjects;

namespace PosFarmacia.Domain.Services;

public sealed class CalculadorCopago
{
    public (Dinero MontoCubierto, Dinero Copago) Calcular(Dinero precioTotal, CoberturaConvenio? cobertura)
    {
        if (cobertura is null)
        {
            return (Dinero.Cero, precioTotal);
        }

        var cubierto = new Dinero(cobertura.PorcentajeCubierto.AplicarSobre(precioTotal.Monto));
        var copago = new Dinero(precioTotal.Monto - cubierto.Monto);
        return (cubierto, copago);
    }
}
