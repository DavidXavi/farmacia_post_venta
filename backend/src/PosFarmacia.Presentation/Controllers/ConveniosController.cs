using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PosFarmacia.Application.DTOs;
using PosFarmacia.Application.UseCases;

namespace PosFarmacia.Presentation.Controllers;

[ApiController]
[Route("api/convenios")]
[Authorize(Roles = "Administrador,OperadorCentral")]
public sealed class ConveniosController(
    RegistrarConvenioUseCase registrarConvenio,
    ConsultarConveniosUseCase consultarConvenios,
    ConfigurarCoberturaUseCase configurarCobertura,
    RegistrarAfiliacionUseCase registrarAfiliacion) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<ConvenioResponse>> Registrar(RegistrarConvenioRequest request, CancellationToken ct) =>
        Ok(await registrarConvenio.EjecutarAsync(request, ct));

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ConvenioResponse>>> Listar(CancellationToken ct) =>
        Ok(await consultarConvenios.EjecutarAsync(ct));

    [HttpPost("{id:guid}/coberturas")]
    public async Task<IActionResult> ConfigurarCobertura(Guid id, ConfigurarCoberturaRequest request, CancellationToken ct)
    {
        await configurarCobertura.EjecutarAsync(id, request, ct);
        return NoContent();
    }

    [HttpPost("afiliaciones")]
    public async Task<ActionResult<AfiliacionResponse>> RegistrarAfiliacion(RegistrarAfiliacionRequest request, CancellationToken ct) =>
        Ok(await registrarAfiliacion.EjecutarAsync(request, ct));
}
