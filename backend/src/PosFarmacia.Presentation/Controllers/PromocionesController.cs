using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PosFarmacia.Application.DTOs;
using PosFarmacia.Application.UseCases;

namespace PosFarmacia.Presentation.Controllers;

[ApiController]
[Route("api/promociones")]
[Authorize]
public sealed class PromocionesController(
    RegistrarPromocionUseCase registrarPromocion,
    ConsultarPromocionesUseCase consultarPromociones,
    DesactivarPromocionUseCase desactivarPromocion) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<ActionResult<PromocionResponse>> Registrar(RegistrarPromocionRequest request, CancellationToken ct) =>
        Ok(await registrarPromocion.EjecutarAsync(request, ct));

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<PromocionResponse>>> Listar(CancellationToken ct) =>
        Ok(await consultarPromociones.EjecutarAsync(ct));

    [HttpPatch("{id:guid}/desactivar")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Desactivar(Guid id, [FromQuery] Guid usuarioId, CancellationToken ct)
    {
        await desactivarPromocion.EjecutarAsync(id, usuarioId, ct);
        return NoContent();
    }
}
