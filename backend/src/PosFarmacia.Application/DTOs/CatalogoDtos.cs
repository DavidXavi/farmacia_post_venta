namespace PosFarmacia.Application.DTOs;

public sealed record NombreRequest(string Nombre);

public sealed record CategoriaResponse(Guid Id, string Nombre);

public sealed record LaboratorioResponse(Guid Id, string Nombre);

public sealed record RegistrarPresentacionRequest(string Nombre, string UnidadMedida);

public sealed record PresentacionResponse(Guid Id, string Nombre, string UnidadMedida);

public sealed record RegistrarFormaPagoRequest(string Nombre, string Tipo);

public sealed record FormaPagoResponse(Guid Id, string Nombre, string Tipo, bool Activo);

public sealed record RegistrarLocalRequest(string Nombre, string Direccion);

public sealed record LocalResponse(Guid Id, string Nombre, string Direccion, bool Activo);

public sealed record RegistrarCajaRequest(string Nombre, Guid LocalId);

public sealed record CajaResponse(Guid Id, string Nombre, Guid LocalId, bool Activa);

public sealed record RegistrarReglaIncentivoRequest(
    string Nombre,
    Guid? ProductoId,
    Guid? CategoriaId,
    decimal MontoPorUnidad,
    DateOnly FechaInicio,
    DateOnly FechaFin);

public sealed record ReglaIncentivoResponse(
    Guid Id,
    string Nombre,
    Guid? ProductoId,
    Guid? CategoriaId,
    decimal MontoPorUnidad,
    DateOnly FechaInicio,
    DateOnly FechaFin,
    bool Activa);
