using PosFarmacia.Domain.ValueObjects;

namespace PosFarmacia.Domain.Entities;

public sealed class NotaCredito : Entidad
{
    private NotaCredito() { }

    public NotaCredito(Guid ventaId, Guid comprobanteId, Guid usuarioId, string motivo, Dinero montoTotal)
    {
        VentaId = ventaId;
        ComprobanteId = comprobanteId;
        UsuarioId = usuarioId;
        Motivo = motivo;
        MontoTotal = montoTotal;
        Fecha = DateTime.UtcNow;
    }

    public Guid VentaId { get; private set; }

    public Guid ComprobanteId { get; private set; }

    public Guid UsuarioId { get; private set; }

    public string Motivo { get; private set; } = string.Empty;

    public Dinero MontoTotal { get; private set; } = Dinero.Cero;

    public DateTime Fecha { get; private set; }
}
