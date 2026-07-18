using PosFarmacia.Application.DTOs;
using PosFarmacia.Domain.Entities;

namespace PosFarmacia.Application.Mappers;

public static class InventarioMappers
{
    public static InventarioResponse ToResponse(this Inventario i) =>
        new(i.ProductoId, i.LocalId, i.CantidadActual.Valor, i.ActualizadoEn);
}
