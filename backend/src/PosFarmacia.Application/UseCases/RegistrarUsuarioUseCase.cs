using PosFarmacia.Application.DTOs;
using PosFarmacia.Application.Ports;
using PosFarmacia.Domain.Entities;
using PosFarmacia.Domain.Enums;
using PosFarmacia.Domain.Exceptions;
using PosFarmacia.Domain.Repositories;

namespace PosFarmacia.Application.UseCases;

public sealed class RegistrarUsuarioUseCase(IUsuarioRepository usuarios, IRolRepository roles, IPasswordHasher passwordHasher, IUnitOfWork unitOfWork)
{
    public async Task<UsuarioResponse> EjecutarAsync(RegistrarUsuarioRequest request, CancellationToken ct = default)
    {
        if (await usuarios.ObtenerPorNombreUsuarioAsync(request.NombreUsuario, ct) is not null)
        {
            throw new ValorInvalidoException("Ya existe un usuario con ese nombre de usuario.");
        }

        var usuario = new Usuario(request.NombreUsuario, passwordHasher.Hash(request.Password), request.LocalId);
        var nombresRoles = new List<string>();

        foreach (var nombreRol in request.Roles)
        {
            var rol = await roles.ObtenerPorNombreAsync(Enum.Parse<RolNombre>(nombreRol), ct)
                ?? throw new EntidadNoEncontradaException($"El rol {nombreRol} no existe.");
            usuario.AsignarRol(rol.Id);
            nombresRoles.Add(rol.Nombre.ToString());
        }

        await usuarios.AgregarAsync(usuario, ct);
        await unitOfWork.GuardarCambiosAsync(ct);

        return new UsuarioResponse(usuario.Id, usuario.NombreUsuario, usuario.LocalId, usuario.Estado.ToString(), nombresRoles);
    }
}
