namespace PosFarmacia.Application.DTOs;

public sealed record InventarioResponse(Guid ProductoId, Guid LocalId, int CantidadActual, DateTime ActualizadoEn);
