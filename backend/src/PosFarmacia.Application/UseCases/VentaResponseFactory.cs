using PosFarmacia.Application.DTOs;
using PosFarmacia.Application.Mappers;
using PosFarmacia.Domain.Entities;
using PosFarmacia.Domain.Repositories;

namespace PosFarmacia.Application.UseCases;

internal static class VentaResponseFactory
{
    public static async Task<VentaResponse> ConstruirAsync(Venta venta, IProductoRepository productos, CancellationToken ct)
    {
        var nombres = new Dictionary<Guid, string>();
        foreach (var productoId in venta.Detalles.Select(d => d.ProductoId).Distinct())
        {
            var producto = await productos.ObtenerPorIdAsync(productoId, ct);
            nombres[productoId] = producto?.NombreComercial ?? string.Empty;
        }

        return venta.ToResponse(nombres);
    }
}
