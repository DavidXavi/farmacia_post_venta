using PosFarmacia.Domain.ValueObjects;

namespace PosFarmacia.Domain.Entities;

public sealed class IncentivoVenta : Entidad
{
    private IncentivoVenta() { }

    public IncentivoVenta(Guid reglaIncentivoId, Guid usuarioId, Guid ventaId, Guid detalleVentaId, Cantidad cantidad, Dinero montoCalculado)
    {
        ReglaIncentivoId = reglaIncentivoId;
        UsuarioId = usuarioId;
        VentaId = ventaId;
        DetalleVentaId = detalleVentaId;
        Cantidad = cantidad;
        MontoCalculado = montoCalculado;
        Fecha = DateTime.UtcNow;
    }

    public Guid ReglaIncentivoId { get; private set; }

    public Guid UsuarioId { get; private set; }

    public Guid VentaId { get; private set; }

    public Guid DetalleVentaId { get; private set; }

    public Cantidad Cantidad { get; private set; } = Cantidad.Cero;

    public Dinero MontoCalculado { get; private set; } = Dinero.Cero;

    public DateTime Fecha { get; private set; }
}
