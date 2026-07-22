using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PosFarmacia.Application.DTOs;
using PosFarmacia.Application.UseCases;

namespace PosFarmacia.Presentation.Controllers;

[ApiController]
[Route("api/devoluciones")]
[Authorize(Roles = "Administrador,Cajero")]
public sealed class DevolucionesController(
    RegistrarDevolucionUseCase registrarDevolucion,
    ConsultarDevolucionesUseCase consultarDevoluciones) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<DevolucionResponse>> Registrar(RegistrarDevolucionRequest request, CancellationToken ct) =>
        Ok(await registrarDevolucion.EjecutarAsync(request, ct));

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<DevolucionResponse>>> Listar([FromQuery] Guid ventaId, CancellationToken ct) =>
        Ok(await consultarDevoluciones.EjecutarAsync(ventaId, ct));
}


