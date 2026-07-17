using PosFarmacia.Application.DTOs;
using PosFarmacia.Domain.Entities;

namespace PosFarmacia.Application.Mappers;

public static class ProductoMappers
{
    public static ProductoResponse ToResponse(this Producto p) => new(
        p.Id,
        p.CodigoInterno.Valor,
        p.CodigoBarras?.Valor,
        p.NombreComercial,
        p.Descripcion,
        p.TipoProducto.ToString(),
        p.CategoriaId,
        p.LaboratorioId,
        p.PresentacionId,
        p.PrecioVenta.Monto,
        p.EsControlado,
        p.RequiereReceta,
        p.TipoRecetaRequerida?.ToString(),
        p.Estado.ToString());
}
