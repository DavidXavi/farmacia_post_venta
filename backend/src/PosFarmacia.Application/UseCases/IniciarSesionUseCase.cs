using PosFarmacia.Application.DTOs;
using PosFarmacia.Application.Ports;
using PosFarmacia.Domain.Exceptions;
using PosFarmacia.Domain.Repositories;

namespace PosFarmacia.Application.UseCases;

public sealed class IniciarSesionUseCase(
    IUsuarioRepository usuarios,
    IRolRepository roles,
    IPasswordHasher passwordHasher,
    IJwtTokenGenerator tokenGenerator)
{
    public async Task<LoginResponse> EjecutarAsync(LoginRequest request, CancellationToken ct = default)
    {
        var usuario = await usuarios.ObtenerPorNombreUsuarioAsync(request.NombreUsuario, ct)
            ?? throw new ValorInvalidoException("Usuario o contrasena invalidos.");

        if (!usuario.EstaActivo || !passwordHasher.Verify(request.Password, usuario.PasswordHash))
        {
            throw new ValorInvalidoException("Usuario o contrasena invalidos.");
        }

        var nombresRoles = new List<string>();
        foreach (var usuarioRol in usuario.Roles)
        {
            var rol = await roles.ObtenerPorIdAsync(usuarioRol.RolId, ct);
            if (rol is not null)
            {
                nombresRoles.Add(rol.Nombre.ToString());
            }
        }

        var token = tokenGenerator.Generar(usuario, nombresRoles);
        return new LoginResponse(token, usuario.Id, usuario.NombreUsuario, nombresRoles, usuario.LocalId);
    }
}
