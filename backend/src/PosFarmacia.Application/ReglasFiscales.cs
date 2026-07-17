using PosFarmacia.Domain.ValueObjects;

namespace PosFarmacia.Application;

/// ponytail: tasa unica de IGV (Peru); si la farmacia necesita tasas por producto, esto se vuelve una columna configurable.
public static class ReglasFiscales
{
    public static readonly Porcentaje IgvPorcentaje = new(18m);

    public const int MesesPreventivosVencimiento = 3;
}
