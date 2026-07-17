using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PosFarmacia.Application.DTOs;
using PosFarmacia.Application.UseCases;

namespace PosFarmacia.Presentation.Controllers;

[ApiController]
[Route("api/notas-credito")]
[Authorize]
public sealed class NotasCreditoController(EmitirNotaCreditoUseCase emitirNotaCredito) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<NotaCreditoResponse>> Emitir(EmitirNotaCreditoRequest request, CancellationToken ct) =>
        Ok(await emitirNotaCredito.EjecutarAsync(request, ct));
}
