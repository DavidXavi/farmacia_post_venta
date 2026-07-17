using PosFarmacia.Domain.ValueObjects;

namespace PosFarmacia.Domain.Entities;

public sealed class AplicacionPromocion : Entidad
{
    private AplicacionPromocion() { }

    public AplicacionPromocion(Guid ventaId, Guid detalleVentaId, Guid promocionId, Dinero montoDescuento)
    {
        VentaId = ventaId;
        DetalleVentaId = detalleVentaId;
        PromocionId = promocionId;
        MontoDescuento = montoDescuento;
    }

    public Guid VentaId { get; private set; }

    public Guid DetalleVentaId { get; private set; }

    public Guid PromocionId { get; private set; }

    public Dinero MontoDescuento { get; private set; } = Dinero.Cero;
}
