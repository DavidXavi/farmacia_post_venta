using PosFarmacia.Domain.ValueObjects;

namespace PosFarmacia.Domain.Entities;

public sealed class CoberturaConvenio : Entidad
{
    private CoberturaConvenio() { }

    internal CoberturaConvenio(Guid convenioId, Guid productoId, Porcentaje porcentajeCubierto)
    {
        ConvenioId = convenioId;
        ProductoId = productoId;
        PorcentajeCubierto = porcentajeCubierto;
    }

    public Guid ConvenioId { get; private set; }

    public Guid ProductoId { get; private set; }

    public Porcentaje PorcentajeCubierto { get; private set; } = Porcentaje.Cero;

    internal void ActualizarPorcentaje(Porcentaje porcentaje) => PorcentajeCubierto = porcentaje;
}
