using PosFarmacia.Application.DTOs;
using PosFarmacia.Domain.Entities;
using PosFarmacia.Domain.Enums;
using PosFarmacia.Domain.Exceptions;
using PosFarmacia.Domain.Repositories;
using PosFarmacia.Domain.Services;
using PosFarmacia.Domain.ValueObjects;

namespace PosFarmacia.Application.UseCases;

public sealed class RegistrarPagoUseCase(
    IVentaRepository ventas,
    IFormaPagoRepository formasPago,
    ILineaCreditoRepository lineasCredito,
    IMovimientoCreditoRepository movimientosCredito,
    IProductoRepository productos,
    ValidadorLineaCredito validadorLineaCredito,
    IUnitOfWork unitOfWork,
    TimeProvider reloj)
{
    public async Task<VentaResponse> EjecutarAsync(Guid ventaId, RegistrarPagoRequest request, CancellationToken ct = default)
    {
        var venta = await ventas.ObtenerPorIdAsync(ventaId, ct)
            ?? throw new EntidadNoEncontradaException("La venta indicada no existe.");

        var formaPago = await formasPago.ObtenerPorIdAsync(request.FormaPagoId, ct)
            ?? throw new EntidadNoEncontradaException("La forma de pago indicada no existe.");

        var monto = new Dinero(request.Monto);

        if (formaPago.Tipo == TipoFormaPago.CreditoFarmacia)
        {
            if (venta.ClienteId is null)
            {
                throw new LineaCreditoInvalidaException("El cliente debe identificarse con su DNI para pagar a credito.");
            }

            var lineaCredito = await lineasCredito.ObtenerPorClienteAsync(venta.ClienteId.Value, ct)
                ?? throw new LineaCreditoInvalidaException("El cliente no cuenta con una linea de credito registrada.");

            var hoy = DateOnly.FromDateTime(reloj.GetUtcNow().UtcDateTime);
            validadorLineaCredito.ValidarParaConsumo(lineaCredito, monto, hoy);
            lineaCredito.Consumir(monto);

            var movimiento = new MovimientoCredito(lineaCredito.Id, venta.Id, TipoMovimientoCredito.Consumo, monto);
            await movimientosCredito.AgregarAsync(movimiento, ct);
            venta.AsignarLineaCredito(lineaCredito.Id);
        }

        venta.RegistrarPago(formaPago.Id, monto, request.CodigoAutorizacion);
        await unitOfWork.GuardarCambiosAsync(ct);

        return await VentaResponseFactory.ConstruirAsync(venta, productos, ct);
    }
}
