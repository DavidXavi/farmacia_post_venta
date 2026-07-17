namespace PosFarmacia.Application.DTOs;

public sealed record VentasDiariasFiltro(DateOnly? Fecha, Guid? LocalId, Guid? CajaId, Guid? UsuarioId, Guid? ClienteId);

public sealed record ReporteIncentivosFiltro(DateOnly Desde, DateOnly Hasta, Guid? UsuarioId);

public sealed record IncentivoResumenResponse(Guid UsuarioId, Guid ProductoId, int CantidadVendida, string ReglaAplicada, decimal MontoTotal);

public sealed record LoteProximoAVencerResponse(Guid LoteId, string Codigo, Guid ProductoId, DateOnly FechaVencimiento, int CantidadDisponible);
