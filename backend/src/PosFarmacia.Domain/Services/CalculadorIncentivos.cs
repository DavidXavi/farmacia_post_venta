using PosFarmacia.Domain.Entities;
using PosFarmacia.Domain.ValueObjects;

namespace PosFarmacia.Domain.Services;

public sealed class CalculadorIncentivos
{
    public Dinero Calcular(ReglaIncentivo regla, Cantidad cantidadVendida) =>
        new(regla.MontoPorUnidad.Monto * cantidadVendida.Valor);
}
