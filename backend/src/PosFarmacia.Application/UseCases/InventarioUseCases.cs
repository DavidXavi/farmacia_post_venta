using PosFarmacia.Application.DTOs;
using PosFarmacia.Application.Mappers;
using PosFarmacia.Domain.Entities;
using PosFarmacia.Domain.Repositories;
using PosFarmacia.Domain.ValueObjects;

namespace PosFarmacia.Application.UseCases;

/// Recalcula el inventario consolidado de un producto+local desde la suma de sus lotes (nunca es fuente de verdad,
/// se recalcula siempre desde Lote para evitar que el rollup se desincronice del stock real).
public sealed class SincronizarInventarioService(ILoteRepository lotes, IInventarioRepository inventarios)
{
    public async Task SincronizarAsync(Guid productoId, Guid localId, CancellationToken ct = default)
    {
        var todosLosLotes = await lotes.ObtenerPorProductoYLocalAsync(productoId, localId, ct);
        var total = new Cantidad(todosLosLotes.Sum(l => l.CantidadDisponible.Valor));

        var inventario = await inventarios.ObtenerPorProductoYLocalAsync(productoId, localId, ct);
        if (inventario is null)
        {
            await inventarios.AgregarAsync(new Inventario(productoId, localId, total), ct);
        }
        else
        {
            inventario.Actualizar(total);
        }
    }
}

public sealed class ConsultarInventarioUseCase(IInventarioRepository inventarios)
{
    public async Task<IReadOnlyList<InventarioResponse>> EjecutarAsync(Guid localId, CancellationToken ct = default) =>
        (await inventarios.ObtenerPorLocalAsync(localId, ct)).Select(i => i.ToResponse()).ToList();
}
