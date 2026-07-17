using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PosFarmacia.Application.DTOs;
using PosFarmacia.Application.UseCases;

namespace PosFarmacia.Presentation.Controllers;

[ApiController]
[Route("api/auditoria")]
[Authorize(Roles = "Administrador")]
public sealed class AuditoriaController(ConsultarAuditoriaUseCase consultarAuditoria) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AuditoriaResponse>>> Listar([FromQuery] AuditoriaFiltro filtro, CancellationToken ct) =>
        Ok(await consultarAuditoria.EjecutarAsync(filtro, ct));
}
