using PosFarmacia.Application.DTOs;
using PosFarmacia.Domain.Entities;

namespace PosFarmacia.Application.Mappers;

public static class LoteMappers
{
    public static LoteResponse ToResponse(this Lote l) => new(
        l.Id,
        l.Codigo.Valor,
        l.ProductoId,
        l.FechaVencimiento.Valor,
        l.CantidadRecibida.Valor,
        l.CantidadDisponible.Valor,
        l.Costo?.Monto,
        l.LocalId,
        l.Estado.ToString());
}
