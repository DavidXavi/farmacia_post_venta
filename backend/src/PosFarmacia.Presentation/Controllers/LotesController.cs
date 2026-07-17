using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PosFarmacia.Application.DTOs;
using PosFarmacia.Application.UseCases;

namespace PosFarmacia.Presentation.Controllers;

[ApiController]
[Route("api/lotes")]
[Authorize]
public sealed class LotesController(
    RegistrarLoteUseCase registrarLote,
    ConsultarLotesUseCase consultarLotes,
    BloquearLoteUseCase bloquearLote,
    RetirarLoteUseCase retirarLote) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "Administrador,EncargadoInventario")]
    public async Task<ActionResult<LoteResponse>> Registrar(RegistrarLoteRequest request, CancellationToken ct) =>
        Ok(await registrarLote.EjecutarAsync(request, ct));

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<LoteResponse>>> Listar([FromQuery] Guid? productoId, CancellationToken ct) =>
        Ok(await consultarLotes.EjecutarAsync(productoId, ct));

    [HttpPatch("{id:guid}/bloquear")]
    [Authorize(Roles = "Administrador,EncargadoInventario,OperadorCentral")]
    public async Task<IActionResult> Bloquear(Guid id, CancellationToken ct)
    {
        await bloquearLote.EjecutarAsync(id, ct);
        return NoContent();
    }

    [HttpPatch("{id:guid}/retirar")]
    [Authorize(Roles = "Administrador,EncargadoInventario,OperadorCentral")]
    public async Task<IActionResult> Retirar(Guid id, CancellationToken ct)
    {
        await retirarLote.EjecutarAsync(id, ct);
        return NoContent();
    }
}
