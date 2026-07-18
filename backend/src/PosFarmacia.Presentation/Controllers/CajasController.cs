using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PosFarmacia.Application.DTOs;
using PosFarmacia.Application.UseCases;

namespace PosFarmacia.Presentation.Controllers;

[ApiController]
[Route("api/cajas")]
[Authorize]
public sealed class CajasController(
    RegistrarCajaUseCase registrarCaja,
    ConsultarCajasUseCase consultarCajas,
    AbrirCajaUseCase abrirCaja,
    CerrarCajaUseCase cerrarCaja,
    ObtenerSesionActivaUseCase obtenerSesionActiva) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<ActionResult<CajaResponse>> Registrar(RegistrarCajaRequest request, CancellationToken ct) =>
        Ok(await registrarCaja.EjecutarAsync(request, ct));

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CajaResponse>>> Listar(CancellationToken ct) =>
        Ok(await consultarCajas.EjecutarAsync(ct));

    [HttpPost("{cajaId:guid}/aperturas")]
    public async Task<ActionResult<SesionCajaResponse>> AbrirCaja(Guid cajaId, AbrirCajaRequest request, CancellationToken ct) =>
        Ok(await abrirCaja.EjecutarAsync(cajaId, request, ct));

    [HttpPost("{cajaId:guid}/cierres")]
    public async Task<ActionResult<SesionCajaResponse>> CerrarCaja(Guid cajaId, CerrarCajaRequest request, CancellationToken ct) =>
        Ok(await cerrarCaja.EjecutarAsync(cajaId, request, ct));

    [HttpGet("{cajaId:guid}/sesion-activa")]
    public async Task<ActionResult<SesionCajaResponse>> SesionActiva(Guid cajaId, CancellationToken ct)
    {
        var sesion = await obtenerSesionActiva.EjecutarAsync(cajaId, ct);
        return sesion is null ? NotFound() : Ok(sesion);
    }
}


