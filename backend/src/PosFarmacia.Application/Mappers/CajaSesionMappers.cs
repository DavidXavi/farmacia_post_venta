using PosFarmacia.Application.DTOs;
using PosFarmacia.Domain.Entities;

namespace PosFarmacia.Application.Mappers;

public static class CajaSesionMappers
{
    public static SesionCajaResponse ToResponse(this SesionCaja s) => new(
        s.Id,
        s.CajaId,
        s.UsuarioId,
        s.FechaApertura,
        s.MontoInicial.Monto,
        s.FechaCierre,
        s.MontoEsperado?.Monto,
        s.MontoDeclarado?.Monto,
        s.Diferencia?.Monto,
        s.ObservacionCierre,
        s.Estado.ToString());
}
