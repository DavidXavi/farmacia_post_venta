using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PosFarmacia.Application.DTOs;
using PosFarmacia.Application.UseCases;

namespace PosFarmacia.Presentation.Controllers;

[ApiController]
[Route("api/productos")]
[Authorize]
public sealed class ProductosController(
    RegistrarProductoUseCase registrarProducto,
    ActualizarProductoUseCase actualizarProducto,
    DarDeBajaProductoUseCase darDeBajaProducto,
    ConsultarProductosUseCase consultarProductos,
    ObtenerProductoUseCase obtenerProducto,
    ConsultarStockVendibleUseCase consultarStockVendible) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<ActionResult<ProductoResponse>> Registrar(RegistrarProductoRequest request, CancellationToken ct) =>
        Ok(await registrarProducto.EjecutarAsync(request, ct));

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ProductoResponse>>> Buscar(
        [FromQuery] string? texto, [FromQuery] Guid? categoriaId, [FromQuery] Guid? laboratorioId, CancellationToken ct) =>
        Ok(await consultarProductos.EjecutarAsync(texto, categoriaId, laboratorioId, ct));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProductoResponse>> ObtenerPorId(Guid id, CancellationToken ct) =>
        Ok(await obtenerProducto.EjecutarAsync(id, ct));

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Administrador")]
    public async Task<ActionResult<ProductoResponse>> Actualizar(Guid id, ActualizarProductoRequest request, CancellationToken ct) =>
        Ok(await actualizarProducto.EjecutarAsync(id, request, ct));

    [HttpPatch("{id:guid}/dar-de-baja")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> DarDeBaja(Guid id, CancellationToken ct)
    {
        await darDeBajaProducto.EjecutarAsync(id, ct);
        return NoContent();
    }

    [HttpGet("{productoId:guid}/stock-vendible")]
    public async Task<ActionResult<StockVendibleResponse>> StockVendible(Guid productoId, [FromQuery] Guid localId, CancellationToken ct) =>
        Ok(await consultarStockVendible.EjecutarAsync(productoId, localId, ct));
}
