using PosFarmacia.Application.DTOs;
using PosFarmacia.Domain.Exceptions;
using PosFarmacia.Domain.Repositories;
using PosFarmacia.Domain.Services;
using PosFarmacia.Domain.ValueObjects;

namespace PosFarmacia.Application.UseCases;

public sealed class AgregarProductoAVentaUseCase(
    IVentaRepository ventas,
    IProductoRepository productos,
    ICajaRepository cajas,
    ILoteRepository lotes,
    IRecetaRepository recetas,
    IUnitOfWork unitOfWork,
    ValidadorReceta validadorReceta,
    TimeProvider reloj)
{
    public async Task<VentaResponse> EjecutarAsync(Guid ventaId, AgregarProductoRequest request, CancellationToken ct = default)
    {
        var venta = await ventas.ObtenerPorIdAsync(ventaId, ct)
            ?? throw new EntidadNoEncontradaException("La venta indicada no existe.");

        var producto = await productos.ObtenerPorIdAsync(request.ProductoId, ct)
            ?? throw new EntidadNoEncontradaException("El producto indicado no existe.");

        if (!producto.EstaVendible)
        {
            throw new ValorInvalidoException("El producto no se encuentra disponible para la venta.");
        }

        var caja = await cajas.ObtenerPorIdAsync(venta.CajaId, ct)
            ?? throw new EntidadNoEncontradaException("La caja de la venta no existe.");

        var hoy = DateOnly.FromDateTime(reloj.GetUtcNow().UtcDateTime);
        var cantidad = new Cantidad(request.Cantidad);

        var stockVendible = (await lotes.ObtenerVendiblesOrdenadosFefoAsync(request.ProductoId, caja.LocalId, hoy, ct))
            .Sum(l => l.CantidadDisponible.Valor);
        if (stockVendible < request.Cantidad)
        {
            throw new StockInsuficienteException();
        }

        Guid? recetaId = null;
        if (producto.EsControlado)
        {
            if (request.RecetaId is null)
            {
                throw new RecetaInvalidaException("Este medicamento controlado requiere una receta validada.");
            }

            var receta = await recetas.ObtenerPorIdAsync(request.RecetaId.Value, ct)
                ?? throw new EntidadNoEncontradaException("La receta indicada no existe.");

            validadorReceta.ValidarParaDispensacion(receta, request.ProductoId, hoy);
            recetaId = receta.Id;
        }

        venta.AgregarDetalle(request.ProductoId, cantidad, producto.PrecioVenta, ReglasFiscales.IgvPorcentaje, recetaId);
        await unitOfWork.GuardarCambiosAsync(ct);

        return await VentaResponseFactory.ConstruirAsync(venta, productos, ct);
    }
}
