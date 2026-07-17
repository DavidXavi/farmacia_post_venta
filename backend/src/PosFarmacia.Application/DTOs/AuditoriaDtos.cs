namespace PosFarmacia.Application.DTOs;

public sealed record AuditoriaFiltro(DateOnly? Fecha, string? Entidad, Guid? UsuarioId);

public sealed record AuditoriaResponse(
    Guid Id,
    DateTime Fecha,
    Guid UsuarioId,
    string Accion,
    string Entidad,
    string EntidadId,
    string Detalle);
