using PosFarmacia.Application.DTOs;
using PosFarmacia.Domain.Entities;

namespace PosFarmacia.Application.Mappers;

public static class PromocionMappers
{
    public static PromocionResponse ToResponse(this Promocion p) => new(
        p.Id,
        p.Nombre,
        p.Descripcion,
        p.TipoBeneficio.ToString(),
        p.ValorBeneficio,
        p.RequiereCliente,
        p.CantidadMinima.Valor,
        p.Vigencia.Inicio,
        p.Vigencia.Fin,
        p.Activa);

    public static PromocionAplicableResponse ToAplicableResponse(this Promocion p) => new(
        p.Id, p.Nombre, p.TipoBeneficio.ToString(), p.ValorBeneficio, p.RequiereCliente);
}
