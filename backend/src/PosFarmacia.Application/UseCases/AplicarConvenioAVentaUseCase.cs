using PosFarmacia.Application.DTOs;
using PosFarmacia.Application.Ports;
using PosFarmacia.Domain.Enums;
using PosFarmacia.Domain.Exceptions;
using PosFarmacia.Domain.Repositories;
using PosFarmacia.Domain.Services;
using PosFarmacia.Domain.ValueObjects;

namespace PosFarmacia.Application.UseCases;

public sealed class AplicarConvenioAVentaUseCase(
    IVentaRepository ventas,
    IFormaPagoRepository formasPago,
    ISeguroClient seguroClient,
    CalculadorCopago calculadorCopago,
    IUnitOfWork unitOfWork,
    TimeProvider reloj)
{
    public async Task<CopagoResponse> EjecutarAsync(Guid ventaId, AplicarConvenioRequest request, CancellationToken ct = default)
    {
        var venta = await ventas.ObtenerPorIdAsync(ventaId, ct)
            ?? throw new EntidadNoEncontradaException("La venta indicada no existe.");

        if (venta.ClienteId is null)
        {
            throw new ConvenioNoDisponibleException("El cliente debe identificarse con su DNI para usar un convenio de seguro.");
        }

        var hoy = DateOnly.FromDateTime(reloj.GetUtcNow().UtcDateTime);
        var convenio = await seguroClient.ConsultarConvenioVigenteAsync(venta.ClienteId.Value, request.ConvenioId, hoy, ct);

        if (convenio is null)
        {
            return new CopagoResponse(request.ConvenioId, 0m, venta.Total.Monto, null);
        }

        var montoCubiertoTotal = Dinero.Cero;
        foreach (var detalle in venta.Detalles)
        {
            var cobertura = convenio.ObtenerCoberturaPara(detalle.ProductoId);
            var (montoCubierto, _) = calculadorCopago.Calcular(detalle.Subtotal, cobertura);
            montoCubiertoTotal += montoCubierto;
        }

        venta.AsignarConvenio(convenio.Id);

        if (montoCubiertoTotal.Monto > 0)
        {
            var formaPagoCopago = (await formasPago.ObtenerTodosAsync(ct)).FirstOrDefault(f => f.Tipo == TipoFormaPago.CopagoSeguro)
                ?? throw new ConvenioNoDisponibleException("No existe una forma de pago configurada para copago de seguro.");

            venta.RegistrarPago(formaPagoCopago.Id, montoCubiertoTotal);
        }

        await unitOfWork.GuardarCambiosAsync(ct);

        var copagoCliente = new Dinero(venta.Total.Monto - montoCubiertoTotal.Monto);
        return new CopagoResponse(convenio.Id, montoCubiertoTotal.Monto, copagoCliente.Monto, null);
    }
}
