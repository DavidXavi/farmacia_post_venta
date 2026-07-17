using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PosFarmacia.Application.DTOs;
using PosFarmacia.Application.UseCases;

namespace PosFarmacia.Presentation.Controllers;

[ApiController]
[Route("api/ventas")]
[Authorize(Roles = "Administrador,Cajero")]
public sealed class VentasController(
    IniciarVentaUseCase iniciarVenta,
    AgregarProductoAVentaUseCase agregarProducto,
    EvaluarPromocionesUseCase evaluarPromociones,
    SeleccionarPromocionUseCase seleccionarPromocion,
    IdentificarClienteEnVentaUseCase identificarCliente,
    AplicarConvenioAVentaUseCase aplicarConvenio,
    RegistrarPagoUseCase registrarPago,
    ConfirmarVentaUseCase confirmarVenta,
    AnularVentaUseCase anularVenta,
    ObtenerVentaUseCase obtenerVenta,
    ConsultarVentasDiariasUseCase consultarVentasDiarias) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<VentaResponse>> Iniciar(IniciarVentaRequest request, CancellationToken ct) =>
        Ok(await iniciarVenta.EjecutarAsync(request, ct));

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<VentaResponse>>> Listar([FromQuery] VentasDiariasFiltro filtro, CancellationToken ct) =>
        Ok(await consultarVentasDiarias.EjecutarAsync(filtro, ct));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<VentaResponse>> ObtenerPorId(Guid id, CancellationToken ct) =>
        Ok(await obtenerVenta.EjecutarAsync(id, ct));

    [HttpPost("{id:guid}/detalles")]
    public async Task<ActionResult<VentaResponse>> AgregarProducto(Guid id, AgregarProductoRequest request, CancellationToken ct) =>
        Ok(await agregarProducto.EjecutarAsync(id, request, ct));

    [HttpGet("{id:guid}/promociones-disponibles")]
    public async Task<ActionResult<IReadOnlyList<PromocionAplicableResponse>>> PromocionesDisponibles(
        Guid id, [FromQuery] Guid detalleVentaId, CancellationToken ct) =>
        Ok(await evaluarPromociones.EjecutarAsync(id, detalleVentaId, ct));

    [HttpPatch("{ventaId:guid}/detalles/{detalleId:guid}/promocion")]
    public async Task<ActionResult<VentaResponse>> SeleccionarPromocion(
        Guid ventaId, Guid detalleId, SeleccionarPromocionRequest request, CancellationToken ct) =>
        Ok(await seleccionarPromocion.EjecutarAsync(ventaId, detalleId, request, ct));

    [HttpPost("{id:guid}/cliente")]
    public async Task<ActionResult<VentaResponse>> IdentificarCliente(Guid id, IdentificarClienteRequest request, CancellationToken ct) =>
        Ok(await identificarCliente.EjecutarAsync(id, request, ct));

    [HttpPost("{id:guid}/convenio")]
    public async Task<ActionResult<CopagoResponse>> AplicarConvenio(Guid id, AplicarConvenioRequest request, CancellationToken ct) =>
        Ok(await aplicarConvenio.EjecutarAsync(id, request, ct));

    [HttpPost("{id:guid}/pagos")]
    public async Task<ActionResult<VentaResponse>> RegistrarPago(Guid id, RegistrarPagoRequest request, CancellationToken ct) =>
        Ok(await registrarPago.EjecutarAsync(id, request, ct));

    [HttpPost("{id:guid}/confirmar")]
    public async Task<ActionResult<VentaResponse>> Confirmar(Guid id, ConfirmarVentaRequest request, CancellationToken ct) =>
        Ok(await confirmarVenta.EjecutarAsync(id, request, ct));

    [HttpPost("{id:guid}/anular")]
    public async Task<ActionResult<VentaResponse>> Anular(Guid id, AnularVentaRequest request, CancellationToken ct) =>
        Ok(await anularVenta.EjecutarAsync(id, request, ct));
}
