using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PosFarmacia.Application.DTOs;
using PosFarmacia.Application.UseCases;

namespace PosFarmacia.Presentation.Controllers;

[ApiController]
[Route("api/inventarios")]
[Authorize]
public sealed class InventariosController(ConsultarInventarioUseCase consultarInventario) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<InventarioResponse>>> Listar([FromQuery] Guid localId, CancellationToken ct) =>
        Ok(await consultarInventario.EjecutarAsync(localId, ct));
}
