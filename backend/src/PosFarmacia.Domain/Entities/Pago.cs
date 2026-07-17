using PosFarmacia.Domain.ValueObjects;

namespace PosFarmacia.Domain.Entities;

public sealed class Pago : Entidad
{
    private Pago() { }

    public Pago(Guid ventaId, Guid formaPagoId, Dinero monto, string? codigoAutorizacion = null)
    {
        VentaId = ventaId;
        FormaPagoId = formaPagoId;
        Monto = monto;
        CodigoAutorizacion = codigoAutorizacion;
        Fecha = DateTime.UtcNow;
    }

    public Guid VentaId { get; private set; }

    public Guid FormaPagoId { get; private set; }

    public Dinero Monto { get; private set; } = Dinero.Cero;

    public string? CodigoAutorizacion { get; private set; }

    public DateTime Fecha { get; private set; }
}
