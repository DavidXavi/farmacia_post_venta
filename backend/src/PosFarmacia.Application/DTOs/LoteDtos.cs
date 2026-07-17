namespace PosFarmacia.Application.DTOs;

public sealed record RegistrarLoteRequest(
    string Codigo,
    Guid ProductoId,
    DateOnly FechaVencimiento,
    int CantidadRecibida,
    Guid LocalId,
    decimal? Costo);

public sealed record LoteResponse(
    Guid Id,
    string Codigo,
    Guid ProductoId,
    DateOnly FechaVencimiento,
    int CantidadRecibida,
    int CantidadDisponible,
    decimal? Costo,
    Guid LocalId,
    string Estado);

public sealed record StockVendibleResponse(Guid ProductoId, int CantidadTotalVendible, IReadOnlyCollection<LoteResponse> Lotes);
