using PosFarmacia.Domain.Enums;
using PosFarmacia.Domain.ValueObjects;

namespace PosFarmacia.Domain.Entities;

public sealed class MovimientoCredito : Entidad
{
    private MovimientoCredito() { }

    public MovimientoCredito(Guid lineaCreditoId, Guid? ventaId, TipoMovimientoCredito tipo, Dinero monto)
    {
        LineaCreditoId = lineaCreditoId;
        VentaId = ventaId;
        Tipo = tipo;
        Monto = monto;
        Fecha = DateTime.UtcNow;
    }

    public Guid LineaCreditoId { get; private set; }

    public Guid? VentaId { get; private set; }

    public TipoMovimientoCredito Tipo { get; private set; }

    public Dinero Monto { get; private set; } = Dinero.Cero;

    public DateTime Fecha { get; private set; }
}
