using PosFarmacia.Application.DTOs;
using PosFarmacia.Application.Mappers;
using PosFarmacia.Domain.Entities;
using PosFarmacia.Domain.Enums;
using PosFarmacia.Domain.Exceptions;
using PosFarmacia.Domain.Repositories;
using PosFarmacia.Domain.ValueObjects;

namespace PosFarmacia.Application.UseCases;

public sealed class RegistrarProductoUseCase(IProductoRepository productos, IUnitOfWork unitOfWork)
{
    public async Task<ProductoResponse> EjecutarAsync(RegistrarProductoRequest request, CancellationToken ct = default)
    {
        var producto = new Producto(
            new CodigoProducto(request.CodigoInterno),
            request.NombreComercial,
            request.Descripcion,
            Enum.Parse<TipoProducto>(request.TipoProducto),
            request.CategoriaId,
            request.LaboratorioId,
            request.PresentacionId,
            new Dinero(request.PrecioVenta),
            request.EsControlado,
            request.RequiereReceta,
            request.TipoRecetaRequerida is null ? null : Enum.Parse<TipoReceta>(request.TipoRecetaRequerida),
            request.CodigoBarras is null ? null : new CodigoBarras(request.CodigoBarras));

        await productos.AgregarAsync(producto, ct);
        await unitOfWork.GuardarCambiosAsync(ct);
        return producto.ToResponse();
    }
}

public sealed class ActualizarProductoUseCase(IProductoRepository productos, IUnitOfWork unitOfWork)
{
    public async Task<ProductoResponse> EjecutarAsync(Guid productoId, ActualizarProductoRequest request, CancellationToken ct = default)
    {
        var producto = await productos.ObtenerPorIdAsync(productoId, ct)
            ?? throw new EntidadNoEncontradaException("El producto indicado no existe.");

        producto.ActualizarDatos(request.NombreComercial, request.Descripcion, new Dinero(request.PrecioVenta));
        await unitOfWork.GuardarCambiosAsync(ct);
        return producto.ToResponse();
    }
}

public sealed class DarDeBajaProductoUseCase(IProductoRepository productos, IUnitOfWork unitOfWork)
{
    public async Task EjecutarAsync(Guid productoId, CancellationToken ct = default)
    {
        var producto = await productos.ObtenerPorIdAsync(productoId, ct)
            ?? throw new EntidadNoEncontradaException("El producto indicado no existe.");

        producto.DarDeBaja();
        await unitOfWork.GuardarCambiosAsync(ct);
    }
}

public sealed class ConsultarProductosUseCase(IProductoRepository productos)
{
    public async Task<IReadOnlyList<ProductoResponse>> EjecutarAsync(string? texto, Guid? categoriaId, Guid? laboratorioId, CancellationToken ct = default)
    {
        var resultado = await productos.BuscarAsync(texto, categoriaId, laboratorioId, ct);
        return resultado.Select(p => p.ToResponse()).ToList();
    }
}

public sealed class ObtenerProductoUseCase(IProductoRepository productos)
{
    public async Task<ProductoResponse> EjecutarAsync(Guid productoId, CancellationToken ct = default)
    {
        var producto = await productos.ObtenerPorIdAsync(productoId, ct)
            ?? throw new EntidadNoEncontradaException("El producto indicado no existe.");
        return producto.ToResponse();
    }
}
