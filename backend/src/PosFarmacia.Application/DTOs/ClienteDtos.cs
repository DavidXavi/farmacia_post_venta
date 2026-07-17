namespace PosFarmacia.Application.DTOs;

public sealed record RegistrarClienteRequest(
    string Dni,
    string Nombres,
    string Apellidos,
    DateOnly? FechaNacimiento,
    string? Telefono,
    string? Correo,
    string? Direccion);

public sealed record ClienteResponse(
    Guid Id,
    string Dni,
    string Nombres,
    string Apellidos,
    DateOnly? FechaNacimiento,
    string? Telefono,
    string? Correo,
    string? Direccion,
    string Estado);
