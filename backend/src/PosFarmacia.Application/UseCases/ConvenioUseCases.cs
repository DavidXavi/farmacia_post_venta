using PosFarmacia.Application.DTOs;
using PosFarmacia.Application.Mappers;
using PosFarmacia.Domain.Entities;
using PosFarmacia.Domain.Exceptions;
using PosFarmacia.Domain.Repositories;
using PosFarmacia.Domain.ValueObjects;

namespace PosFarmacia.Application.UseCases;

public sealed class RegistrarConvenioUseCase(IConvenioSeguroRepository convenios, IUnitOfWork unitOfWork)
{
    public async Task<ConvenioResponse> EjecutarAsync(RegistrarConvenioRequest request, CancellationToken ct = default)
    {
        var convenio = new ConvenioSeguro(request.Nombre);
        await convenios.AgregarAsync(convenio, ct);
        await unitOfWork.GuardarCambiosAsync(ct);
        return convenio.ToResponse();
    }
}

public sealed class ConfigurarCoberturaUseCase(IConvenioSeguroRepository convenios, IUnitOfWork unitOfWork)
{
    public async Task EjecutarAsync(Guid convenioId, ConfigurarCoberturaRequest request, CancellationToken ct = default)
    {
        var convenio = await convenios.ObtenerPorIdAsync(convenioId, ct)
            ?? throw new EntidadNoEncontradaException("El convenio indicado no existe.");

        convenio.ConfigurarCobertura(request.ProductoId, new Porcentaje(request.PorcentajeCubierto));
        await unitOfWork.GuardarCambiosAsync(ct);
    }
}

public sealed class RegistrarAfiliacionUseCase(IAfiliacionClienteRepository afiliaciones, IUnitOfWork unitOfWork)
{
    public async Task<AfiliacionResponse> EjecutarAsync(RegistrarAfiliacionRequest request, CancellationToken ct = default)
    {
        var afiliacion = new AfiliacionCliente(request.ClienteId, request.ConvenioId, request.VigenciaInicio, request.VigenciaFin);
        await afiliaciones.AgregarAsync(afiliacion, ct);
        await unitOfWork.GuardarCambiosAsync(ct);
        return afiliacion.ToResponse();
    }
}

public sealed class ConsultarConvenioDeClienteUseCase(IAfiliacionClienteRepository afiliaciones)
{
    public async Task<IReadOnlyList<AfiliacionResponse>> EjecutarAsync(Guid clienteId, CancellationToken ct = default)
    {
        var resultado = await afiliaciones.ObtenerPorClienteAsync(clienteId, ct);
        return resultado.Select(a => a.ToResponse()).ToList();
    }
}

public sealed class ConsultarConveniosUseCase(IConvenioSeguroRepository convenios)
{
    public async Task<IReadOnlyList<ConvenioResponse>> EjecutarAsync(CancellationToken ct = default) =>
        (await convenios.ObtenerTodosAsync(ct)).Select(c => c.ToResponse()).ToList();
}
