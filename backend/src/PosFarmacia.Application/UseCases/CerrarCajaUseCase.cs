using PosFarmacia.Application.DTOs;
using PosFarmacia.Application.Mappers;
using PosFarmacia.Domain.Enums;
using PosFarmacia.Domain.Exceptions;
using PosFarmacia.Domain.Repositories;
using PosFarmacia.Domain.ValueObjects;

namespace PosFarmacia.Application.UseCases;

public sealed class CerrarCajaUseCase(
    ISesionCajaRepository sesiones,
    IVentaRepository ventas,
    IFormaPagoRepository formasPago,
    IUnitOfWork unitOfWork)
{
    public async Task<SesionCajaResponse> EjecutarAsync(Guid cajaId, CerrarCajaRequest request, CancellationToken ct = default)
    {
        var sesion = await sesiones.ObtenerSesionActivaAsync(cajaId, ct)
            ?? throw new EntidadNoEncontradaException("La caja no tiene una sesion abierta.");

        var ventasDeSesion = await ventas.ObtenerPorSesionCajaAsync(sesion.Id, ct);
        var formas = await formasPago.ObtenerTodosAsync(ct);
        var idsEfectivo = formas.Where(f => f.Tipo == TipoFormaPago.Efectivo).Select(f => f.Id).ToHashSet();

        var ingresosEfectivo = ventasDeSesion
            .Where(v => v.Estado == EstadoVenta.Confirmada)
            .SelectMany(v => v.Pagos)
            .Where(p => idsEfectivo.Contains(p.FormaPagoId))
            .Sum(p => p.Monto.Monto);

        var montoEsperado = new Dinero(sesion.MontoInicial.Monto + ingresosEfectivo);
        sesion.Cerrar(montoEsperado, new Dinero(request.MontoDeclarado), request.Observacion);

        await unitOfWork.GuardarCambiosAsync(ct);
        return sesion.ToResponse();
    }
}
