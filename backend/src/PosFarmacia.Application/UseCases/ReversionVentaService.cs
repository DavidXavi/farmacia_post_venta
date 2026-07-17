using PosFarmacia.Domain.Entities;
using PosFarmacia.Domain.Enums;
using PosFarmacia.Domain.Repositories;
using PosFarmacia.Domain.Services;
using PosFarmacia.Domain.ValueObjects;

namespace PosFarmacia.Application.UseCases;

/// Logica compartida por AnularVenta y EmitirNotaCredito: revertir stock (RN42/RN43) y linea de credito (RN32).
public sealed class ReversionVentaService(
    ILoteRepository lotes,
    IMovimientoStockRepository movimientosStock,
    ILineaCreditoRepository lineasCredito,
    IMovimientoCreditoRepository movimientosCredito,
    IFormaPagoRepository formasPago,
    ServicioAnulacionVenta servicioAnulacion)
{
    public async Task RevertirStockAsync(Venta venta, Guid usuarioId, CancellationToken ct)
    {
        foreach (var (loteId, cantidad) in servicioAnulacion.ObtenerReversionesDeStock(venta))
        {
            var lote = await lotes.ObtenerPorIdAsync(loteId, ct);
            if (lote is null || !lote.Devolver(cantidad))
            {
                continue;
            }

            var movimiento = new MovimientoStock(loteId, TipoMovimientoStock.ReversionAnulacion, cantidad, usuarioId, venta.Id.ToString());
            await movimientosStock.AgregarAsync(movimiento, ct);
        }
    }

    public async Task RevertirCreditoAsync(Venta venta, CancellationToken ct)
    {
        if (venta.LineaCreditoId is null)
        {
            return;
        }

        var lineaCredito = await lineasCredito.ObtenerPorIdAsync(venta.LineaCreditoId.Value, ct);
        if (lineaCredito is null)
        {
            return;
        }

        var idsFormasCredito = (await formasPago.ObtenerTodosAsync(ct))
            .Where(f => f.Tipo == TipoFormaPago.CreditoFarmacia)
            .Select(f => f.Id)
            .ToHashSet();

        var montoCredito = venta.Pagos.Where(p => idsFormasCredito.Contains(p.FormaPagoId)).Sum(p => p.Monto.Monto);
        if (montoCredito <= 0)
        {
            return;
        }

        var monto = new Dinero(montoCredito);
        lineaCredito.Revertir(monto);
        await movimientosCredito.AgregarAsync(new MovimientoCredito(lineaCredito.Id, venta.Id, TipoMovimientoCredito.Reversion, monto), ct);
    }
}
