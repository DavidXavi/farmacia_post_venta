using PosFarmacia.Application.DTOs;
using PosFarmacia.Domain.Entities;

namespace PosFarmacia.Application.Mappers;

public static class RecetaMappers
{
    public static RecetaResponse ToResponse(this Receta r) => new(
        r.Id,
        r.Numero.Valor,
        r.Tipo.ToString(),
        r.FechaEmision,
        r.FechaVencimiento,
        r.ProductoId,
        r.ClienteId,
        r.Estado.ToString(),
        r.RetenidaEnBotica);

    public static ValidacionRecetaResponse ToResponse(this ValidacionReceta v) => new(
        v.Id, v.RecetaId, v.UsuarioValidadorId, v.Fecha, v.Resultado.ToString(), v.Observaciones);
}
