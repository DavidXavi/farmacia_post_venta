using PosFarmacia.Domain.Entities;

namespace PosFarmacia.Domain.Repositories;

public interface ISesionCajaRepository : IRepositorio<SesionCaja>
{
    Task<SesionCaja?> ObtenerSesionActivaAsync(Guid cajaId, CancellationToken ct = default);
}
