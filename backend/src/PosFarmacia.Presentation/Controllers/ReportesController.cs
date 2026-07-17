using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PosFarmacia.Application.DTOs;
using PosFarmacia.Application.UseCases;

namespace PosFarmacia.Presentation.Controllers;

[ApiController]
[Route("api/reportes")]
[Authorize(Roles = "Administrador")]
public sealed class ReportesController(
    ConsultarVentasDiariasUseCase consultarVentasDiarias,
    GenerarReporteIncentivosUseCase generarReporteIncentivos,
    ConsultarLotesProximosAVencerUseCase consultarLotesProximosAVencer) : ControllerBase
{
    [HttpGet("ventas-diarias")]
    public async Task<ActionResult<IReadOnlyList<VentaResponse>>> VentasDiarias([FromQuery] VentasDiariasFiltro filtro, CancellationToken ct) =>
        Ok(await consultarVentasDiarias.EjecutarAsync(filtro, ct));

    [HttpGet("incentivos")]
    public async Task<ActionResult<IReadOnlyList<IncentivoResumenResponse>>> Incentivos([FromQuery] ReporteIncentivosFiltro filtro, CancellationToken ct) =>
        Ok(await generarReporteIncentivos.EjecutarAsync(filtro, ct));

    [HttpGet("lotes-proximos-a-vencer")]
    public async Task<ActionResult<IReadOnlyList<LoteProximoAVencerResponse>>> LotesProximosAVencer([FromQuery] int diasHorizonte, CancellationToken ct) =>
        Ok(await consultarLotesProximosAVencer.EjecutarAsync(diasHorizonte == 0 ? 90 : diasHorizonte, ct));
}
