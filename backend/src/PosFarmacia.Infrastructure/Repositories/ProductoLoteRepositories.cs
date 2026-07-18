using Microsoft.EntityFrameworkCore;
using PosFarmacia.Domain.Entities;
using PosFarmacia.Domain.Enums;
using PosFarmacia.Domain.Repositories;
using PosFarmacia.Domain.ValueObjects;
using PosFarmacia.Infrastructure.Persistence;

namespace PosFarmacia.Infrastructure.Repositories;

public sealed class ProductoRepository(PosFarmaciaDbContext contexto) : RepositorioBase<Producto>(contexto), IProductoRepository
{
    public async Task<IReadOnlyList<Producto>> BuscarAsync(string? texto, Guid? categoriaId, Guid? laboratorioId, CancellationToken ct = default)
    {
        var query = Contexto.Productos.AsQueryable();

        if (!string.IsNullOrWhiteSpace(texto))
        {
            query = query.Where(p => p.NombreComercial.Contains(texto));
        }

        if (categoriaId is not null)
        {
            query = query.Where(p => p.CategoriaId == categoriaId);
        }

        if (laboratorioId is not null)
        {
            query = query.Where(p => p.LaboratorioId == laboratorioId);
        }

        return await query.ToListAsync(ct);
    }

    public async Task<Producto?> ObtenerPorCodigoBarrasAsync(string codigoBarras, CancellationToken ct = default) =>
        await Contexto.Productos.FirstOrDefaultAsync(p => p.CodigoBarras == new CodigoBarras(codigoBarras), ct);
}

public sealed class LoteRepository(PosFarmaciaDbContext contexto) : RepositorioBase<Lote>(contexto), ILoteRepository
{
    public async Task<IReadOnlyList<Lote>> ObtenerVendiblesOrdenadosFefoAsync(Guid productoId, Guid localId, DateOnly hoy, CancellationToken ct = default)
    {
        var candidatos = await Contexto.Lotes
            .Where(l => l.ProductoId == productoId && l.LocalId == localId && l.Estado == EstadoLote.Disponible)
            .ToListAsync(ct);

        return candidatos
            .Where(l => l.EsVendible(hoy))
            .OrderBy(l => l.FechaVencimiento.Valor)
            .ToList();
    }

    public async Task<IReadOnlyList<Lote>> ObtenerProximosAVencerAsync(DateOnly hoy, int diasHorizonte, CancellationToken ct = default)
    {
        var candidatos = await Contexto.Lotes.Where(l => l.Estado == EstadoLote.Disponible).ToListAsync(ct);
        var limite = hoy.AddDays(diasHorizonte);

        return candidatos
            .Where(l => !l.FechaVencimiento.EstaVencida(hoy) && l.FechaVencimiento.Valor <= limite)
            .OrderBy(l => l.FechaVencimiento.Valor)
            .ToList();
    }

    public async Task<IReadOnlyList<Lote>> ObtenerPorProductoYLocalAsync(Guid productoId, Guid localId, CancellationToken ct = default) =>
        await Contexto.Lotes.Where(l => l.ProductoId == productoId && l.LocalId == localId).ToListAsync(ct);
}

public sealed class MovimientoStockRepository(PosFarmaciaDbContext contexto) : RepositorioBase<MovimientoStock>(contexto), IMovimientoStockRepository;

public sealed class InventarioRepository(PosFarmaciaDbContext contexto) : RepositorioBase<Inventario>(contexto), IInventarioRepository
{
    public async Task<Inventario?> ObtenerPorProductoYLocalAsync(Guid productoId, Guid localId, CancellationToken ct = default) =>
        await Contexto.Inventarios.FirstOrDefaultAsync(i => i.ProductoId == productoId && i.LocalId == localId, ct);

    public async Task<IReadOnlyList<Inventario>> ObtenerPorLocalAsync(Guid localId, CancellationToken ct = default) =>
        await Contexto.Inventarios.Where(i => i.LocalId == localId).ToListAsync(ct);
}
