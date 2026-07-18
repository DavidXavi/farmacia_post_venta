using PosFarmacia.Application.DTOs;
using PosFarmacia.Application.Mappers;
using PosFarmacia.Domain.Entities;
using PosFarmacia.Domain.Enums;
using PosFarmacia.Domain.Repositories;
using PosFarmacia.Domain.ValueObjects;

namespace PosFarmacia.Application.UseCases;

public sealed class RegistrarCategoriaUseCase(ICategoriaRepository categorias, IUnitOfWork unitOfWork)
{
    public async Task<CategoriaResponse> EjecutarAsync(NombreRequest request, CancellationToken ct = default)
    {
        var categoria = new Categoria(request.Nombre);
        await categorias.AgregarAsync(categoria, ct);
        await unitOfWork.GuardarCambiosAsync(ct);
        return categoria.ToResponse();
    }
}

public sealed class ConsultarCategoriasUseCase(ICategoriaRepository categorias)
{
    public async Task<IReadOnlyList<CategoriaResponse>> EjecutarAsync(CancellationToken ct = default) =>
        (await categorias.ObtenerTodosAsync(ct)).Select(c => c.ToResponse()).ToList();
}

public sealed class RegistrarLaboratorioUseCase(ILaboratorioRepository laboratorios, IUnitOfWork unitOfWork)
{
    public async Task<LaboratorioResponse> EjecutarAsync(NombreRequest request, CancellationToken ct = default)
    {
        var laboratorio = new Laboratorio(request.Nombre);
        await laboratorios.AgregarAsync(laboratorio, ct);
        await unitOfWork.GuardarCambiosAsync(ct);
        return laboratorio.ToResponse();
    }
}

public sealed class ConsultarLaboratoriosUseCase(ILaboratorioRepository laboratorios)
{
    public async Task<IReadOnlyList<LaboratorioResponse>> EjecutarAsync(CancellationToken ct = default) =>
        (await laboratorios.ObtenerTodosAsync(ct)).Select(l => l.ToResponse()).ToList();
}

public sealed class RegistrarPresentacionUseCase(IPresentacionRepository presentaciones, IUnitOfWork unitOfWork)
{
    public async Task<PresentacionResponse> EjecutarAsync(RegistrarPresentacionRequest request, CancellationToken ct = default)
    {
        var presentacion = new Presentacion(request.Nombre, request.UnidadMedida);
        await presentaciones.AgregarAsync(presentacion, ct);
        await unitOfWork.GuardarCambiosAsync(ct);
        return presentacion.ToResponse();
    }
}

public sealed class ConsultarPresentacionesUseCase(IPresentacionRepository presentaciones)
{
    public async Task<IReadOnlyList<PresentacionResponse>> EjecutarAsync(CancellationToken ct = default) =>
        (await presentaciones.ObtenerTodosAsync(ct)).Select(p => p.ToResponse()).ToList();
}

public sealed class RegistrarFormaPagoUseCase(IFormaPagoRepository formasPago, IUnitOfWork unitOfWork)
{
    public async Task<FormaPagoResponse> EjecutarAsync(RegistrarFormaPagoRequest request, CancellationToken ct = default)
    {
        var formaPago = new FormaPago(request.Nombre, Enum.Parse<TipoFormaPago>(request.Tipo));
        await formasPago.AgregarAsync(formaPago, ct);
        await unitOfWork.GuardarCambiosAsync(ct);
        return formaPago.ToResponse();
    }
}

public sealed class ConsultarFormasPagoUseCase(IFormaPagoRepository formasPago)
{
    public async Task<IReadOnlyList<FormaPagoResponse>> EjecutarAsync(CancellationToken ct = default) =>
        (await formasPago.ObtenerTodosAsync(ct)).Select(f => f.ToResponse()).ToList();
}

public sealed class RegistrarLocalUseCase(ILocalRepository locales, IUnitOfWork unitOfWork)
{
    public async Task<LocalResponse> EjecutarAsync(RegistrarLocalRequest request, CancellationToken ct = default)
    {
        var local = new Local(request.Nombre, request.Direccion);
        await locales.AgregarAsync(local, ct);
        await unitOfWork.GuardarCambiosAsync(ct);
        return local.ToResponse();
    }
}

public sealed class ConsultarLocalesUseCase(ILocalRepository locales)
{
    public async Task<IReadOnlyList<LocalResponse>> EjecutarAsync(CancellationToken ct = default) =>
        (await locales.ObtenerTodosAsync(ct)).Select(l => l.ToResponse()).ToList();
}

public sealed class RegistrarCajaUseCase(ICajaRepository cajas, IUnitOfWork unitOfWork)
{
    public async Task<CajaResponse> EjecutarAsync(RegistrarCajaRequest request, CancellationToken ct = default)
    {
        var caja = new Caja(request.Nombre, request.LocalId);
        await cajas.AgregarAsync(caja, ct);
        await unitOfWork.GuardarCambiosAsync(ct);
        return caja.ToResponse();
    }
}

public sealed class ConsultarCajasUseCase(ICajaRepository cajas)
{
    public async Task<IReadOnlyList<CajaResponse>> EjecutarAsync(CancellationToken ct = default) =>
        (await cajas.ObtenerTodosAsync(ct)).Select(c => c.ToResponse()).ToList();
}

public sealed class RegistrarReglaIncentivoUseCase(IReglaIncentivoRepository reglas, IUnitOfWork unitOfWork)
{
    public async Task<ReglaIncentivoResponse> EjecutarAsync(RegistrarReglaIncentivoRequest request, CancellationToken ct = default)
    {
        var regla = new ReglaIncentivo(request.Nombre, request.ProductoId, request.CategoriaId,
            new Dinero(request.MontoPorUnidad), request.FechaInicio, request.FechaFin);
        await reglas.AgregarAsync(regla, ct);
        await unitOfWork.GuardarCambiosAsync(ct);
        return regla.ToResponse();
    }
}

public sealed class ConsultarReglasIncentivoUseCase(IReglaIncentivoRepository reglas)
{
    public async Task<IReadOnlyList<ReglaIncentivoResponse>> EjecutarAsync(CancellationToken ct = default) =>
        (await reglas.ObtenerTodosAsync(ct)).Select(r => r.ToResponse()).ToList();
}
