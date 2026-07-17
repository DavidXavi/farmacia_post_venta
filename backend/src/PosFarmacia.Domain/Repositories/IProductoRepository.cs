using PosFarmacia.Domain.Entities;

namespace PosFarmacia.Domain.Repositories;

public interface IProductoRepository : IRepositorio<Producto>
{
    Task<IReadOnlyList<Producto>> BuscarAsync(string? texto, Guid? categoriaId, Guid? laboratorioId, CancellationToken ct = default);

    Task<Producto?> ObtenerPorCodigoBarrasAsync(string codigoBarras, CancellationToken ct = default);
}
