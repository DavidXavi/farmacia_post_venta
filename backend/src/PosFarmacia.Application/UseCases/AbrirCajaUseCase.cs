using PosFarmacia.Application.DTOs;
using PosFarmacia.Application.Mappers;
using PosFarmacia.Domain.Entities;
using PosFarmacia.Domain.Exceptions;
using PosFarmacia.Domain.Repositories;
using PosFarmacia.Domain.ValueObjects;

namespace PosFarmacia.Application.UseCases;

public sealed class AbrirCajaUseCase(ICajaRepository cajas, ISesionCajaRepository sesiones, IUnitOfWork unitOfWork)
{
    public async Task<SesionCajaResponse> EjecutarAsync(Guid cajaId, AbrirCajaRequest request, CancellationToken ct = default)
    {
        _ = await cajas.ObtenerPorIdAsync(cajaId, ct) ?? throw new EntidadNoEncontradaException("La caja indicada no existe.");

        var sesionActiva = await sesiones.ObtenerSesionActivaAsync(cajaId, ct);
        if (sesionActiva is not null)
        {
            throw new CajaCerradaException("La caja ya tiene una sesion abierta.");
        }

        var sesion = new SesionCaja(cajaId, request.UsuarioId, new Dinero(request.MontoInicial));
        await sesiones.AgregarAsync(sesion, ct);
        await unitOfWork.GuardarCambiosAsync(ct);

        return sesion.ToResponse();
    }
}
