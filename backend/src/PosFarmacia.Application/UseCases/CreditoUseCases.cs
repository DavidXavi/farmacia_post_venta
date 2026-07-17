using PosFarmacia.Application.DTOs;
using PosFarmacia.Application.Mappers;
using PosFarmacia.Domain.Entities;
using PosFarmacia.Domain.Exceptions;
using PosFarmacia.Domain.Repositories;
using PosFarmacia.Domain.ValueObjects;

namespace PosFarmacia.Application.UseCases;

public sealed class RegistrarLineaCreditoUseCase(ILineaCreditoRepository lineasCredito, IUnitOfWork unitOfWork)
{
    public async Task<LineaCreditoResponse> EjecutarAsync(RegistrarLineaCreditoRequest request, CancellationToken ct = default)
    {
        var linea = new LineaCredito(request.ClienteId, new Dinero(request.MontoAutorizado), request.VigenciaInicio, request.VigenciaFin);
        await lineasCredito.AgregarAsync(linea, ct);
        await unitOfWork.GuardarCambiosAsync(ct);
        return linea.ToResponse();
    }
}

public sealed class ConsultarLineaCreditoUseCase(ILineaCreditoRepository lineasCredito)
{
    public async Task<LineaCreditoResponse?> EjecutarAsync(Guid clienteId, CancellationToken ct = default)
    {
        var linea = await lineasCredito.ObtenerPorClienteAsync(clienteId, ct);
        return linea?.ToResponse();
    }
}
