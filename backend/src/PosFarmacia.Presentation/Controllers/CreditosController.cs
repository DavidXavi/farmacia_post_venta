using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PosFarmacia.Application.DTOs;
using PosFarmacia.Application.UseCases;

namespace PosFarmacia.Presentation.Controllers;

[ApiController]
[Route("api/lineas-credito")]
[Authorize(Roles = "Administrador,OperadorCentral")]
public sealed class CreditosController(RegistrarLineaCreditoUseCase registrarLineaCredito) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<LineaCreditoResponse>> Registrar(RegistrarLineaCreditoRequest request, CancellationToken ct) =>
        Ok(await registrarLineaCredito.EjecutarAsync(request, ct));
}
