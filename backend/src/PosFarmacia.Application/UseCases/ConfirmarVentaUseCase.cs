using PosFarmacia.Application.DTOs;
using PosFarmacia.Domain.Entities;
using PosFarmacia.Domain.Enums;
using PosFarmacia.Domain.Exceptions;
using PosFarmacia.Domain.Repositories;
using PosFarmacia.Domain.Services;
using PosFarmacia.Domain.ValueObjects;

namespace PosFarmacia.Application.UseCases;

public sealed class ConfirmarVentaUseCase(
    IVentaRepository ventas,
    ICajaRepository cajas,
    IProductoRepository productos,
    ILoteRepository lotes,
    IMovimientoStockRepository movimientosStock,
    IReglaIncentivoRepository reglasIncentivo,
    IIncentivoVentaRepository incentivosVenta,
    IRecetaRepository recetas,
    AsignadorLotesFEFO asignadorFefo,
    CalculadorIncentivos calculadorIncentivos,
    SincronizarInventarioService sincronizarInventario,
    IUnitOfWork unitOfWork,
    TimeProvider reloj)
{
    public async Task<VentaResponse> EjecutarAsync(Guid ventaId, ConfirmarVentaRequest request, CancellationToken ct = default)
    {
        var venta = await ventas.ObtenerPorIdAsync(ventaId, ct)
            ?? throw new EntidadNoEncontradaException("La venta indicada no existe.");

        var caja = await cajas.ObtenerPorIdAsync(venta.CajaId, ct)
            ?? throw new EntidadNoEncontradaException("La caja de la venta no existe.");

        var hoy = DateOnly.FromDateTime(reloj.GetUtcNow().UtcDateTime);

        // RN33/RN34: asignacion FEFO de lotes por cada linea que aun no tenga stock reservado.
        foreach (var detalle in venta.Detalles)
        {
            var faltante = detalle.Cantidad.Valor - detalle.CantidadAsignadaEnLotes.Valor;
            if (faltante <= 0)
            {
                continue;
            }

            var vendibles = await lotes.ObtenerVendiblesOrdenadosFefoAsync(detalle.ProductoId, caja.LocalId, hoy, ct);
            var asignaciones = asignadorFefo.Asignar(vendibles, new Cantidad(faltante));

            foreach (var (loteId, cantidad) in asignaciones)
            {
                var lote = vendibles.First(l => l.Id == loteId);
                lote.Reservar(cantidad);
                venta.AsignarLoteADetalle(detalle.Id, loteId, cantidad);

                var movimiento = new MovimientoStock(loteId, TipoMovimientoStock.Salida, cantidad, venta.UsuarioId, venta.Id.ToString());
                await movimientosStock.AgregarAsync(movimiento, ct);
                await sincronizarInventario.SincronizarAsync(lote.ProductoId, lote.LocalId, ct);
            }

            var producto = await productos.ObtenerPorIdAsync(detalle.ProductoId, ct);
            if (producto is not null)
            {
                await RegistrarIncentivoSiAplicaAsync(venta, detalle, producto, hoy, ct);
            }
        }

        var numeroCorrelativo = await ventas.ObtenerSiguienteCorrelativoAsync(ct);
        venta.Confirmar(numeroCorrelativo, Enum.Parse<TipoComprobante>(request.TipoComprobante), request.SerieComprobante);

        // RN18: la receta especial retenida queda utilizada y retenida en la botica tras confirmar la venta.
        foreach (var detalle in venta.Detalles.Where(d => d.RecetaId is not null))
        {
            var receta = await recetas.ObtenerPorIdAsync(detalle.RecetaId!.Value, ct);
            receta?.MarcarUtilizadaYRetenida();
        }

        await unitOfWork.GuardarCambiosAsync(ct);
        return await VentaResponseFactory.ConstruirAsync(venta, productos, ct);
    }

    private async Task RegistrarIncentivoSiAplicaAsync(Venta venta, DetalleVenta detalle, Producto producto, DateOnly hoy, CancellationToken ct)
    {
        var reglas = await reglasIncentivo.ObtenerTodosAsync(ct);
        var regla = reglas.FirstOrDefault(r => r.AplicaA(producto.Id, producto.CategoriaId, hoy));
        if (regla is null)
        {
            return;
        }

        var monto = calculadorIncentivos.Calcular(regla, detalle.Cantidad);
        var incentivo = new IncentivoVenta(regla.Id, venta.UsuarioId, venta.Id, detalle.Id, detalle.Cantidad, monto);
        await incentivosVenta.AgregarAsync(incentivo, ct);
    }
}
