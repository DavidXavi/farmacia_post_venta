using PosFarmacia.Application.DTOs;
using PosFarmacia.Application.Mappers;
using PosFarmacia.Application.Ports;
using PosFarmacia.Domain.Entities;
using PosFarmacia.Domain.Enums;
using PosFarmacia.Domain.Exceptions;
using PosFarmacia.Domain.Repositories;
using PosFarmacia.Domain.Services;
using PosFarmacia.Domain.ValueObjects;

namespace PosFarmacia.Application.UseCases;

public sealed class RegistrarDevolucionUseCase(
    IVentaRepository ventas,
    IProductoRepository productos,
    IDevolucionRepository devoluciones,
    ILoteRepository lotes,
    IMovimientoStockRepository movimientosStock,
    ValidadorDevolucion validador,
    AsignadorReversionesDevolucion asignadorReversiones,
    SincronizarInventarioService sincronizarInventario,
    IAuditService auditService,
    IUnitOfWork unitOfWork,
    TimeProvider reloj)
{
    public async Task<DevolucionResponse> EjecutarAsync(RegistrarDevolucionRequest request, CancellationToken ct = default)
    {
        var venta = await ventas.ObtenerPorIdAsync(request.VentaId, ct)
            ?? throw new EntidadNoEncontradaException("La venta indicada no existe.");

        var hoy = DateOnly.FromDateTime(reloj.GetUtcNow().UtcDateTime);
        var devolucionesPrevias = await devoluciones.ObtenerPorVentaAsync(request.VentaId, ct);
        var yaDevueltoPorDetalle = devolucionesPrevias
            .SelectMany(d => d.Detalles)
            .GroupBy(d => d.DetalleVentaId)
            .ToDictionary(g => g.Key, g => g.Sum(d => d.Cantidad.Valor));

        var devolucion = new Devolucion(venta.Id, request.UsuarioId, request.Motivo);

        foreach (var linea in request.Lineas)
        {
            var detalle = venta.Detalles.FirstOrDefault(d => d.Id == linea.DetalleVentaId)
                ?? throw new EntidadNoEncontradaException("La linea de venta indicada no existe en esta venta.");

            var producto = await productos.ObtenerPorIdAsync(detalle.ProductoId, ct)
                ?? throw new EntidadNoEncontradaException("El producto de la linea de venta no existe.");

            var cantidadYaDevuelta = new Cantidad(yaDevueltoPorDetalle.GetValueOrDefault(detalle.Id, 0));
            var cantidadSolicitada = new Cantidad(linea.Cantidad);

            validador.Validar(venta, producto, detalle.Cantidad, cantidadYaDevuelta, cantidadSolicitada, hoy);

            var montoUnitario = detalle.Subtotal.Monto / detalle.Cantidad.Valor;
            var montoDevuelto = new Dinero(montoUnitario * cantidadSolicitada.Valor);
            devolucion.AgregarLinea(detalle.Id, detalle.ProductoId, cantidadSolicitada, montoDevuelto);

            foreach (var (loteId, cantidad) in asignadorReversiones.Asignar(detalle, cantidadSolicitada))
            {
                var lote = await lotes.ObtenerPorIdAsync(loteId, ct);
                if (lote is null || !lote.Devolver(cantidad))
                {
                    continue;
                }

                await movimientosStock.AgregarAsync(
                    new MovimientoStock(loteId, TipoMovimientoStock.ReversionDevolucion, cantidad, request.UsuarioId, devolucion.Id.ToString()), ct);
                await sincronizarInventario.SincronizarAsync(lote.ProductoId, lote.LocalId, ct);
            }
        }

        await devoluciones.AgregarAsync(devolucion, ct);
        await auditService.RegistrarAsync(
            request.UsuarioId, "RegistrarDevolucion", nameof(Devolucion), devolucion.Id.ToString(), $"Venta {venta.Id}. Motivo: {request.Motivo}", ct: ct);

        await unitOfWork.GuardarCambiosAsync(ct);
        return devolucion.ToResponse();
    }
}

public sealed class ConsultarDevolucionesUseCase(IDevolucionRepository devoluciones)
{
    public async Task<IReadOnlyList<DevolucionResponse>> EjecutarAsync(Guid ventaId, CancellationToken ct = default) =>
        (await devoluciones.ObtenerPorVentaAsync(ventaId, ct)).Select(d => d.ToResponse()).ToList();
}
