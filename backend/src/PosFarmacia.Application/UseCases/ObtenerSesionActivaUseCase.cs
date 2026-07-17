using PosFarmacia.Application.DTOs;
using PosFarmacia.Application.Mappers;
using PosFarmacia.Domain.Repositories;

namespace PosFarmacia.Application.UseCases;

public sealed class ObtenerSesionActivaUseCase(ISesionCajaRepository sesiones)
{
    public async Task<SesionCajaResponse?> EjecutarAsync(Guid cajaId, CancellationToken ct = default)
    {
        var sesion = await sesiones.ObtenerSesionActivaAsync(cajaId, ct);
        return sesion?.ToResponse();
    }
}
