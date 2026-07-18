namespace PosFarmacia.Application.DTOs;

public sealed record RegistrarUsuarioRequest(string NombreUsuario, string Password, Guid LocalId, IReadOnlyCollection<string> Roles);

public sealed record UsuarioResponse(Guid Id, string NombreUsuario, Guid LocalId, string Estado, IReadOnlyCollection<string> Roles);

public sealed record RolResponse(Guid Id, string Nombre, string Descripcion);
