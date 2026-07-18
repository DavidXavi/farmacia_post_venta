namespace PosFarmacia.Application.DTOs;

public sealed record LineaDevolucionRequest(Guid DetalleVentaId, int Cantidad);

public sealed record RegistrarDevolucionRequest(
    Guid VentaId,
    Guid UsuarioId,
    string Motivo,
    IReadOnlyCollection<LineaDevolucionRequest> Lineas);

public sealed record DetalleDevolucionResponse(Guid Id, Guid DetalleVentaId, Guid ProductoId, int Cantidad, decimal MontoDevuelto);

public sealed record DevolucionResponse(
    Guid Id,
    Guid VentaId,
    Guid UsuarioId,
    string Motivo,
    DateTime Fecha,
    decimal Total,
    IReadOnlyCollection<DetalleDevolucionResponse> Detalles);
