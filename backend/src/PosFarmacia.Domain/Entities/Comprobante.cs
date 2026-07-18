using PosFarmacia.Domain.Enums;
using PosFarmacia.Domain.ValueObjects;

namespace PosFarmacia.Domain.Entities;

public sealed class Comprobante : Entidad
{
    private Comprobante() { }

    public Comprobante(Guid ventaId, TipoComprobante tipo, NumeroComprobante numero)
    {
        VentaId = ventaId;
        Tipo = tipo;
        Numero = numero;
        FechaEmision = DateTime.UtcNow;
    }

    public Guid VentaId { get; private set; }

    public TipoComprobante Tipo { get; private set; }

    public NumeroComprobante Numero { get; private set; } = null!;

    public DateTime FechaEmision { get; private set; }
}


