using PosFarmacia.Domain.ValueObjects;

namespace PosFarmacia.Domain.Entities;

public sealed class DetalleVentaLote : Entidad
{
    private DetalleVentaLote() { }

    public DetalleVentaLote(Guid detalleVentaId, Guid loteId, Cantidad cantidadTomada)
    {
        DetalleVentaId = detalleVentaId;
        LoteId = loteId;
        CantidadTomada = cantidadTomada;
    }

    public Guid DetalleVentaId { get; private set; }

    public Guid LoteId { get; private set; }

    public Cantidad CantidadTomada { get; private set; } = Cantidad.Cero;
}
