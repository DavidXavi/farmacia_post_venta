using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PosFarmacia.Application.DTOs;
using PosFarmacia.Application.UseCases;

namespace PosFarmacia.Presentation.Controllers;

[ApiController]
[Route("api/recetas")]
[Authorize]
public sealed class RecetasController(
    RegistrarRecetaUseCase registrarReceta,
    ValidarRecetaUseCase validarReceta,
    ObtenerRecetaUseCase obtenerReceta) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "Administrador,Cajero,QuimicoFarmaceutico")]
    public async Task<ActionResult<RecetaResponse>> Registrar(RegistrarRecetaRequest request, CancellationToken ct) =>
        Ok(await registrarReceta.EjecutarAsync(request, ct));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RecetaResponse>> ObtenerPorId(Guid id, CancellationToken ct) =>
        Ok(await obtenerReceta.EjecutarAsync(id, ct));

    [HttpPost("validaciones")]
    [Authorize(Roles = "Administrador,QuimicoFarmaceutico")]
    public async Task<ActionResult<ValidacionRecetaResponse>> Validar(ValidarRecetaRequest request, CancellationToken ct) =>
        Ok(await validarReceta.EjecutarAsync(request, ct));
}
