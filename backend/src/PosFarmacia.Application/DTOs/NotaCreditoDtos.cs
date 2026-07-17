namespace PosFarmacia.Application.DTOs;

public sealed record EmitirNotaCreditoRequest(Guid VentaId, Guid UsuarioId, string Motivo);

public sealed record NotaCreditoResponse(Guid Id, Guid VentaId, Guid ComprobanteId, Guid UsuarioId, string Motivo, decimal MontoTotal, DateTime Fecha);
