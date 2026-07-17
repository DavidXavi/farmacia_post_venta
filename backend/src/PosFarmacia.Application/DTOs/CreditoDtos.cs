namespace PosFarmacia.Application.DTOs;

public sealed record RegistrarLineaCreditoRequest(Guid ClienteId, decimal MontoAutorizado, DateOnly? VigenciaInicio, DateOnly? VigenciaFin);

public sealed record LineaCreditoResponse(Guid Id, Guid ClienteId, decimal MontoAutorizado, decimal SaldoDisponible, string Estado, DateOnly? VigenciaInicio, DateOnly? VigenciaFin);
