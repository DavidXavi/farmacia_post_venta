using PosFarmacia.Domain.ValueObjects;

namespace PosFarmacia.Domain.Entities;

public sealed class Devolucion : Entidad
{
    private readonly List<DetalleDevolucion> _detalles = [];

    private Devolucion() { }

    public Devolucion(Guid ventaId, Guid usuarioId, string motivo)
    {
        VentaId = ventaId;
        UsuarioId = usuarioId;
        Motivo = motivo;
        Fecha = DateTime.UtcNow;
    }

    public Guid VentaId { get; private set; }

    public Guid UsuarioId { get; private set; }

    public string Motivo { get; private set; } = string.Empty;

    public DateTime Fecha { get; private set; }

    public IReadOnlyCollection<DetalleDevolucion> Detalles => _detalles;

    public Dinero Total => new(_detalles.Sum(d => d.MontoDevuelto.Monto));

    public void AgregarLinea(Guid detalleVentaId, Guid productoId, Cantidad cantidad, Dinero montoDevuelto) =>
        _detalles.Add(new DetalleDevolucion(Id, detalleVentaId, productoId, cantidad, montoDevuelto));
}

public sealed class DetalleDevolucion : Entidad
{
    private DetalleDevolucion() { }

    internal DetalleDevolucion(Guid devolucionId, Guid detalleVentaId, Guid productoId, Cantidad cantidad, Dinero montoDevuelto)
    {
        DevolucionId = devolucionId;
        DetalleVentaId = detalleVentaId;
        ProductoId = productoId;
        Cantidad = cantidad;
        MontoDevuelto = montoDevuelto;
    }

    public Guid DevolucionId { get; private set; }

    public Guid DetalleVentaId { get; private set; }

    public Guid ProductoId { get; private set; }

    public Cantidad Cantidad { get; private set; } = Cantidad.Cero;

    public Dinero MontoDevuelto { get; private set; } = Dinero.Cero;
}
