using PosFarmacia.Application.DTOs;
using PosFarmacia.Application.Mappers;
using PosFarmacia.Domain.Exceptions;
using PosFarmacia.Domain.Repositories;
using PosFarmacia.Domain.Services;

namespace PosFarmacia.Application.UseCases;

public sealed class EvaluarPromocionesUseCase(
    IVentaRepository ventas,
    IPromocionRepository promociones,
    EvaluadorPromociones evaluador,
    TimeProvider reloj)
{
    public async Task<IReadOnlyList<PromocionAplicableResponse>> EjecutarAsync(Guid ventaId, Guid detalleVentaId, CancellationToken ct = default)
    {
        var venta = await ventas.ObtenerPorIdAsync(ventaId, ct)
            ?? throw new EntidadNoEncontradaException("La venta indicada no existe.");

        var detalle = venta.Detalles.FirstOrDefault(d => d.Id == detalleVentaId)
            ?? throw new EntidadNoEncontradaException("La linea de venta indicada no existe.");

        var hoy = DateOnly.FromDateTime(reloj.GetUtcNow().UtcDateTime);
        var candidatas = await promociones.ObtenerVigentesPorProductoAsync(detalle.ProductoId, hoy, ct);
        var aplicables = evaluador.ObtenerAplicables(candidatas, detalle.Cantidad, venta.ClienteId is not null, hoy);

        return aplicables.Select(p => p.ToAplicableResponse()).ToList();
    }
}

public sealed class SeleccionarPromocionUseCase(
    IVentaRepository ventas,
    IPromocionRepository promociones,
    IProductoRepository productos,
    IUnitOfWork unitOfWork,
    TimeProvider reloj)
{
    public async Task<VentaResponse> EjecutarAsync(Guid ventaId, Guid detalleVentaId, SeleccionarPromocionRequest request, CancellationToken ct = default)
    {
        var venta = await ventas.ObtenerPorIdAsync(ventaId, ct)
            ?? throw new EntidadNoEncontradaException("La venta indicada no existe.");

        var detalle = venta.Detalles.FirstOrDefault(d => d.Id == detalleVentaId)
            ?? throw new EntidadNoEncontradaException("La linea de venta indicada no existe.");

        var promocion = await promociones.ObtenerPorIdAsync(request.PromocionId, ct)
            ?? throw new EntidadNoEncontradaException("La promocion indicada no existe.");

        var hoy = DateOnly.FromDateTime(reloj.GetUtcNow().UtcDateTime);
        if (!promocion.EstaVigente(hoy) || !promocion.AplicaAProducto(detalle.ProductoId))
        {
            throw new PromocionInvalidaException("La promocion no es aplicable a esta linea de venta.");
        }

        if (promocion.RequiereCliente && venta.ClienteId is null)
        {
            throw new PromocionInvalidaException("Esta promocion exige que el cliente se identifique con su DNI.");
        }

        if (detalle.Cantidad.Valor < promocion.CantidadMinima.Valor)
        {
            throw new PromocionInvalidaException("La cantidad de la linea no cumple el minimo requerido por la promocion.");
        }

        var descuento = promocion.CalcularDescuento(detalle.PrecioUnitario, detalle.Cantidad);
        venta.AplicarPromocionALinea(detalleVentaId, promocion.Id, descuento, ReglasFiscales.IgvPorcentaje);

        await unitOfWork.GuardarCambiosAsync(ct);
        return await VentaResponseFactory.ConstruirAsync(venta, productos, ct);
    }
}
