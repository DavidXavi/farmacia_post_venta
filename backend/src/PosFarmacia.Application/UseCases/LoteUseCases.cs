using PosFarmacia.Application.DTOs;
using PosFarmacia.Application.Mappers;
using PosFarmacia.Domain.Entities;
using PosFarmacia.Domain.Exceptions;
using PosFarmacia.Domain.Repositories;
using PosFarmacia.Domain.ValueObjects;

namespace PosFarmacia.Application.UseCases;

public sealed class RegistrarLoteUseCase(ILoteRepository lotes, SincronizarInventarioService sincronizarInventario, IUnitOfWork unitOfWork)
{
    public async Task<LoteResponse> EjecutarAsync(RegistrarLoteRequest request, CancellationToken ct = default)
    {
        var lote = new Lote(
            new CodigoLote(request.Codigo),
            request.ProductoId,
            new FechaVencimiento(request.FechaVencimiento),
            new Cantidad(request.CantidadRecibida),
            request.LocalId,
            request.Costo is null ? null : new Dinero(request.Costo.Value));

        await lotes.AgregarAsync(lote, ct);
        // El lote aun no existe en BD (Added): se persiste primero para que el recalculo del inventario lo cuente.
        await unitOfWork.GuardarCambiosAsync(ct);

        await sincronizarInventario.SincronizarAsync(lote.ProductoId, lote.LocalId, ct);
        await unitOfWork.GuardarCambiosAsync(ct);
        return lote.ToResponse();
    }
}

public sealed class ConsultarStockVendibleUseCase(ILoteRepository lotes, TimeProvider reloj)
{
    public async Task<StockVendibleResponse> EjecutarAsync(Guid productoId, Guid localId, CancellationToken ct = default)
    {
        var hoy = DateOnly.FromDateTime(reloj.GetUtcNow().UtcDateTime);
        var vendibles = await lotes.ObtenerVendiblesOrdenadosFefoAsync(productoId, localId, hoy, ct);
        var total = vendibles.Sum(l => l.CantidadDisponible.Valor);
        return new StockVendibleResponse(productoId, total, vendibles.Select(l => l.ToResponse()).ToList());
    }
}

public sealed class BloquearLoteUseCase(ILoteRepository lotes, IUnitOfWork unitOfWork)
{
    public async Task EjecutarAsync(Guid loteId, CancellationToken ct = default)
    {
        var lote = await lotes.ObtenerPorIdAsync(loteId, ct) ?? throw new EntidadNoEncontradaException("El lote indicado no existe.");
        lote.Bloquear();
        await unitOfWork.GuardarCambiosAsync(ct);
    }
}

public sealed class RetirarLoteUseCase(ILoteRepository lotes, IUnitOfWork unitOfWork)
{
    public async Task EjecutarAsync(Guid loteId, CancellationToken ct = default)
    {
        var lote = await lotes.ObtenerPorIdAsync(loteId, ct) ?? throw new EntidadNoEncontradaException("El lote indicado no existe.");
        lote.Retirar();
        await unitOfWork.GuardarCambiosAsync(ct);
    }
}

public sealed class ConsultarLotesUseCase(ILoteRepository lotes)
{
    public async Task<IReadOnlyList<LoteResponse>> EjecutarAsync(Guid? productoId, CancellationToken ct = default)
    {
        var todos = await lotes.ObtenerTodosAsync(ct);
        var filtrados = productoId is null ? todos : todos.Where(l => l.ProductoId == productoId).ToList();
        return filtrados.Select(l => l.ToResponse()).ToList();
    }
}

public sealed class ConsultarLotesProximosAVencerUseCase(ILoteRepository lotes, TimeProvider reloj)
{
    public async Task<IReadOnlyList<LoteProximoAVencerResponse>> EjecutarAsync(int diasHorizonte = 90, CancellationToken ct = default)
    {
        var hoy = DateOnly.FromDateTime(reloj.GetUtcNow().UtcDateTime);
        var proximos = await lotes.ObtenerProximosAVencerAsync(hoy, diasHorizonte, ct);
        return proximos.Select(l => new LoteProximoAVencerResponse(l.Id, l.Codigo.Valor, l.ProductoId, l.FechaVencimiento.Valor, l.CantidadDisponible.Valor)).ToList();
    }
}
