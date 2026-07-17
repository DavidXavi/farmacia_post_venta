using PosFarmacia.Application.DTOs;
using PosFarmacia.Domain.Exceptions;
using PosFarmacia.Domain.Repositories;

namespace PosFarmacia.Application.UseCases;

public sealed class ObtenerVentaUseCase(IVentaRepository ventas, IProductoRepository productos)
{
    public async Task<VentaResponse> EjecutarAsync(Guid ventaId, CancellationToken ct = default)
    {
        var venta = await ventas.ObtenerPorIdAsync(ventaId, ct)
            ?? throw new EntidadNoEncontradaException("La venta indicada no existe.");
        return await VentaResponseFactory.ConstruirAsync(venta, productos, ct);
    }
}

public sealed class ConsultarVentasDiariasUseCase(IVentaRepository ventas, IProductoRepository productos)
{
    public async Task<IReadOnlyList<VentaResponse>> EjecutarAsync(VentasDiariasFiltro filtro, CancellationToken ct = default)
    {
        var resultado = await ventas.BuscarAsync(filtro.Fecha, filtro.LocalId, filtro.CajaId, filtro.UsuarioId, filtro.ClienteId, ct);

        var respuestas = new List<VentaResponse>();
        foreach (var venta in resultado)
        {
            respuestas.Add(await VentaResponseFactory.ConstruirAsync(venta, productos, ct));
        }

        return respuestas;
    }
}
