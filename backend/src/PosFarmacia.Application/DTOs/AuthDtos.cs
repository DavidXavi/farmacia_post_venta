namespace PosFarmacia.Application.DTOs;

public sealed record LoginRequest(string NombreUsuario, string Password);

public sealed record LoginResponse(string Token, Guid UsuarioId, string NombreUsuario, IReadOnlyCollection<string> Roles, Guid LocalId);
