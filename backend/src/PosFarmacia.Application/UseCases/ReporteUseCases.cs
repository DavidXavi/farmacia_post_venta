using PosFarmacia.Application.DTOs;
using PosFarmacia.Application.Mappers;
using PosFarmacia.Domain.Repositories;

namespace PosFarmacia.Application.UseCases;

public sealed class GenerarReporteIncentivosUseCase(IIncentivoVentaRepository incentivos, IReglaIncentivoRepository reglas)
{
    public async Task<IReadOnlyList<IncentivoResumenResponse>> EjecutarAsync(ReporteIncentivosFiltro filtro, CancellationToken ct = default)
    {
        var registros = await incentivos.BuscarAsync(filtro.Desde, filtro.Hasta, filtro.UsuarioId, ct);
        var reglasCache = new Dictionary<Guid, string>();

        var agrupados = registros
            .GroupBy(i => new { i.UsuarioId, ProductoId = i.DetalleVentaId })
            .Select(async g =>
            {
                var primero = g.First();
                if (!reglasCache.TryGetValue(primero.ReglaIncentivoId, out var nombreRegla))
                {
                    var regla = await reglas.ObtenerPorIdAsync(primero.ReglaIncentivoId, ct);
                    nombreRegla = regla?.Nombre ?? string.Empty;
                    reglasCache[primero.ReglaIncentivoId] = nombreRegla;
                }

                return new IncentivoResumenResponse(
                    g.Key.UsuarioId,
                    primero.DetalleVentaId,
                    g.Sum(i => i.Cantidad.Valor),
                    nombreRegla,
                    g.Sum(i => i.MontoCalculado.Monto));
            });

        return (await Task.WhenAll(agrupados)).ToList();
    }
}

public sealed class ConsultarAuditoriaUseCase(IAuditoriaRepository auditoria)
{
    public async Task<IReadOnlyList<AuditoriaResponse>> EjecutarAsync(AuditoriaFiltro filtro, CancellationToken ct = default)
    {
        var registros = await auditoria.BuscarAsync(filtro.Fecha, filtro.Entidad, filtro.UsuarioId, ct);
        return registros.Select(a => a.ToResponse()).ToList();
    }
}
