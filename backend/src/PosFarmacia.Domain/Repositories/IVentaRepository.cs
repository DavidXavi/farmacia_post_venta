using PosFarmacia.Domain.Entities;

namespace PosFarmacia.Domain.Repositories;

public interface IVentaRepository : IRepositorio<Venta>
{
    Task<long> ObtenerSiguienteCorrelativoAsync(CancellationToken ct = default);

    Task<IReadOnlyList<Venta>> BuscarAsync(
        DateOnly? fecha,
        Guid? localId,
        Guid? cajaId,
        Guid? usuarioId,
        Guid? clienteId,
        CancellationToken ct = default);

    Task<IReadOnlyList<Venta>> ObtenerPorSesionCajaAsync(Guid sesionCajaId, CancellationToken ct = default);
}
