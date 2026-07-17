using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PosFarmacia.Application.DTOs;
using PosFarmacia.Application.UseCases;

namespace PosFarmacia.Presentation.Controllers;

[ApiController]
[Route("api/usuarios")]
[Authorize(Roles = "Administrador")]
public sealed class UsuariosController(RegistrarUsuarioUseCase registrarUsuario) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<UsuarioResponse>> Registrar(RegistrarUsuarioRequest request, CancellationToken ct) =>
        Ok(await registrarUsuario.EjecutarAsync(request, ct));
}
