namespace PosFarmacia.Application.DTOs;

public sealed record RegistrarRecetaRequest(
    string Numero,
    string Tipo,
    DateOnly FechaEmision,
    DateOnly? FechaVencimiento,
    Guid ProductoId,
    Guid? ClienteId,
    string DatosPaciente,
    string DatosProfesional,
    string DosisYCantidadAutorizada,
    string? ArchivoRespaldoUrl);

public sealed record ValidarRecetaRequest(Guid RecetaId, Guid UsuarioValidadorId, bool Aprobar, string? Observaciones);

public sealed record RecetaResponse(
    Guid Id,
    string Numero,
    string Tipo,
    DateOnly FechaEmision,
    DateOnly? FechaVencimiento,
    Guid ProductoId,
    Guid? ClienteId,
    string Estado,
    bool RetenidaEnBotica);

public sealed record ValidacionRecetaResponse(Guid Id, Guid RecetaId, Guid UsuarioValidadorId, DateTime Fecha, string Resultado, string? Observaciones);
