namespace PosFarmacia.Application.DTOs;

public sealed record RegistrarConvenioRequest(string Nombre);

public sealed record ConfigurarCoberturaRequest(Guid ProductoId, decimal PorcentajeCubierto);

public sealed record RegistrarAfiliacionRequest(Guid ClienteId, Guid ConvenioId, DateOnly? VigenciaInicio, DateOnly? VigenciaFin);

public sealed record ConvenioResponse(Guid Id, string Nombre, bool Activo);

public sealed record AfiliacionResponse(Guid Id, Guid ClienteId, Guid ConvenioId, string Estado, DateOnly? VigenciaInicio, DateOnly? VigenciaFin);

public sealed record CopagoResponse(Guid ConvenioId, decimal MontoCubierto, decimal Copago, string? CodigoAutorizacion);
