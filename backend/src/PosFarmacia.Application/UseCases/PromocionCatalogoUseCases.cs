using PosFarmacia.Application.DTOs;
using PosFarmacia.Application.Mappers;
using PosFarmacia.Application.Ports;
using PosFarmacia.Domain.Entities;
using PosFarmacia.Domain.Enums;
using PosFarmacia.Domain.Exceptions;
using PosFarmacia.Domain.Repositories;
using PosFarmacia.Domain.ValueObjects;

namespace PosFarmacia.Application.UseCases;

public sealed class RegistrarPromocionUseCase(IPromocionRepository promociones, IUnitOfWork unitOfWork)
{
    public async Task<PromocionResponse> EjecutarAsync(RegistrarPromocionRequest request, CancellationToken ct = default)
    {
        var promocion = new Promocion(
            request.Nombre,
            request.Descripcion,
            Enum.Parse<TipoBeneficioPromocion>(request.TipoBeneficio),
            request.ValorBeneficio,
            request.RequiereCliente,
            new Cantidad(request.CantidadMinima),
            request.FechaInicio,
            request.FechaFin);

        foreach (var productoId in request.ProductosParticipantes)
        {
            promocion.AgregarProductoParticipante(productoId);
        }

        await promociones.AgregarAsync(promocion, ct);
        await unitOfWork.GuardarCambiosAsync(ct);
        return promocion.ToResponse();
    }
}

public sealed class EditarPromocionUseCase(IPromocionRepository promociones, IUnitOfWork unitOfWork)
{
    public async Task<PromocionResponse> EjecutarAsync(Guid promocionId, EditarPromocionRequest request, CancellationToken ct = default)
    {
        var promocion = await promociones.ObtenerPorIdAsync(promocionId, ct)
            ?? throw new EntidadNoEncontradaException("La promocion indicada no existe.");

        promocion.EditarDatos(
            request.Nombre,
            request.Descripcion,
            Enum.Parse<TipoBeneficioPromocion>(request.TipoBeneficio),
            request.ValorBeneficio,
            request.RequiereCliente,
            new Cantidad(request.CantidadMinima),
            request.FechaInicio,
            request.FechaFin,
            request.ProductosParticipantes);

        await unitOfWork.GuardarCambiosAsync(ct);
        return promocion.ToResponse();
    }
}

public sealed class ConsultarPromocionesUseCase(IPromocionRepository promociones)
{
    public async Task<IReadOnlyList<PromocionResponse>> EjecutarAsync(CancellationToken ct = default)
    {
        var resultado = await promociones.ObtenerTodosAsync(ct);
        return resultado.Select(p => p.ToResponse()).ToList();
    }
}

public sealed class DesactivarPromocionUseCase(IPromocionRepository promociones, IAuditService auditService, IUnitOfWork unitOfWork)
{
    public async Task EjecutarAsync(Guid promocionId, Guid usuarioId, CancellationToken ct = default)
    {
        var promocion = await promociones.ObtenerPorIdAsync(promocionId, ct)
            ?? throw new EntidadNoEncontradaException("La promocion indicada no existe.");

        promocion.Desactivar();
        await auditService.RegistrarAsync(usuarioId, "DesactivarPromocion", nameof(Promocion), promocion.Id.ToString(), string.Empty, ct: ct);
        await unitOfWork.GuardarCambiosAsync(ct);
    }
}
