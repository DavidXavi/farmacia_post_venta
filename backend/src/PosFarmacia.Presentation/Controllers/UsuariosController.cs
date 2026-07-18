using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PosFarmacia.Application.DTOs;
using PosFarmacia.Application.UseCases;

namespace PosFarmacia.Presentation.Controllers;

[ApiController]
[Route("api/usuarios")]
[Authorize(Roles = "Administrador")]
public sealed class UsuariosController(RegistrarUsuarioUseCase registrarUsuario, ConsultarUsuariosUseCase consultarUsuarios) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<UsuarioResponse>> Registrar(RegistrarUsuarioRequest request, CancellationToken ct) =>
        Ok(await registrarUsuario.EjecutarAsync(request, ct));

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<UsuarioResponse>>> Listar(CancellationToken ct) =>
        Ok(await consultarUsuarios.EjecutarAsync(ct));
}

[ApiController]
[Route("api/roles")]
[Authorize(Roles = "Administrador")]
public sealed class RolesController(ConsultarRolesUseCase consultarRoles) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RolResponse>>> Listar(CancellationToken ct) =>
        Ok(await consultarRoles.EjecutarAsync(ct));
}
