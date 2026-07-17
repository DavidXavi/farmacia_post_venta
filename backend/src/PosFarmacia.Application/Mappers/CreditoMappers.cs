using PosFarmacia.Application.DTOs;
using PosFarmacia.Domain.Entities;

namespace PosFarmacia.Application.Mappers;

public static class CreditoMappers
{
    public static LineaCreditoResponse ToResponse(this LineaCredito l) => new(
        l.Id, l.ClienteId, l.MontoAutorizado.Monto, l.SaldoDisponible.Monto, l.Estado.ToString(), l.Vigencia.Inicio, l.Vigencia.Fin);
}
