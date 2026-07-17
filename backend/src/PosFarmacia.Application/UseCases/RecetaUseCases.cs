using PosFarmacia.Application.DTOs;
using PosFarmacia.Application.Mappers;
using PosFarmacia.Application.Ports;
using PosFarmacia.Domain.Entities;
using PosFarmacia.Domain.Enums;
using PosFarmacia.Domain.Exceptions;
using PosFarmacia.Domain.Repositories;
using PosFarmacia.Domain.ValueObjects;

namespace PosFarmacia.Application.UseCases;

public sealed class RegistrarRecetaUseCase(IRecetaRepository recetas, IUnitOfWork unitOfWork)
{
    public async Task<RecetaResponse> EjecutarAsync(RegistrarRecetaRequest request, CancellationToken ct = default)
    {
        var receta = new Receta(
            new NumeroReceta(request.Numero),
            Enum.Parse<TipoReceta>(request.Tipo),
            request.FechaEmision,
            request.FechaVencimiento,
            request.ProductoId,
            request.DatosPaciente,
            request.DatosProfesional,
            request.DosisYCantidadAutorizada,
            request.ClienteId,
            request.ArchivoRespaldoUrl);

        await recetas.AgregarAsync(receta, ct);
        await unitOfWork.GuardarCambiosAsync(ct);
        return receta.ToResponse();
    }
}

public sealed class ValidarRecetaUseCase(
    IRecetaRepository recetas,
    IValidacionRecetaRepository validaciones,
    IAuditService auditService,
    IUnitOfWork unitOfWork)
{
    public async Task<ValidacionRecetaResponse> EjecutarAsync(ValidarRecetaRequest request, CancellationToken ct = default)
    {
        var receta = await recetas.ObtenerPorIdAsync(request.RecetaId, ct)
            ?? throw new EntidadNoEncontradaException("La receta indicada no existe.");

        if (request.Aprobar)
        {
            receta.Aprobar();
        }
        else
        {
            receta.Rechazar();
        }

        var validacion = new ValidacionReceta(receta.Id, request.UsuarioValidadorId,
            request.Aprobar ? EstadoReceta.Aprobada : EstadoReceta.Rechazada, request.Observaciones);
        await validaciones.AgregarAsync(validacion, ct);

        await auditService.RegistrarAsync(request.UsuarioValidadorId, "ValidarReceta", nameof(Receta), receta.Id.ToString(),
            request.Observaciones ?? string.Empty, ct: ct);

        await unitOfWork.GuardarCambiosAsync(ct);
        return validacion.ToResponse();
    }
}

public sealed class ObtenerRecetaUseCase(IRecetaRepository recetas)
{
    public async Task<RecetaResponse> EjecutarAsync(Guid recetaId, CancellationToken ct = default)
    {
        var receta = await recetas.ObtenerPorIdAsync(recetaId, ct)
            ?? throw new EntidadNoEncontradaException("La receta indicada no existe.");
        return receta.ToResponse();
    }
}
