namespace PosFarmacia.Application.DTOs;

public sealed record AbrirCajaRequest(Guid UsuarioId, decimal MontoInicial);

public sealed record CerrarCajaRequest(decimal MontoDeclarado, string? Observacion);

public sealed record SesionCajaResponse(
    Guid Id,
    Guid CajaId,
    Guid UsuarioId,
    DateTime FechaApertura,
    decimal MontoInicial,
    DateTime? FechaCierre,
    decimal? MontoEsperado,
    decimal? MontoDeclarado,
    decimal? Diferencia,
    string? ObservacionCierre,
    string Estado);
