using PosFarmacia.Application.DTOs;
using PosFarmacia.Application.Ports;
using PosFarmacia.Domain.Entities;
using PosFarmacia.Domain.Enums;
using PosFarmacia.Domain.Exceptions;
using PosFarmacia.Domain.Repositories;
using PosFarmacia.Domain.Services;

namespace PosFarmacia.Application.UseCases;

public sealed class AnularVentaUseCase(
    IVentaRepository ventas,
    IUsuarioRepository usuarios,
    IProductoRepository productos,
    ServicioAnulacionVenta servicioAnulacion,
    ReversionVentaService reversionVenta,
    IAuditService auditService,
    IUnitOfWork unitOfWork,
    TimeProvider reloj)
{
    public async Task<VentaResponse> EjecutarAsync(Guid ventaId, AnularVentaRequest request, CancellationToken ct = default)
    {
        var venta = await ventas.ObtenerPorIdAsync(ventaId, ct)
            ?? throw new EntidadNoEncontradaException("La venta indicada no existe.");

        var usuario = await usuarios.ObtenerPorIdAsync(request.UsuarioId, ct)
            ?? throw new EntidadNoEncontradaException("El usuario indicado no existe.");

        if (!usuario.TienePermiso(PermisoEspecial.AnularVentas))
        {
            throw new AnulacionNoPermitidaException("El usuario no cuenta con permiso para anular ventas.");
        }

        var hoy = DateOnly.FromDateTime(reloj.GetUtcNow().UtcDateTime);
        if (servicioAnulacion.RequiereNotaCredito(venta, hoy))
        {
            throw new AnulacionNoPermitidaException("La venta corresponde a un dia anterior; debe emitirse una nota de credito.");
        }

        venta.Anular(hoy);

        await reversionVenta.RevertirStockAsync(venta, request.UsuarioId, ct);
        await reversionVenta.RevertirCreditoAsync(venta, ct);

        await auditService.RegistrarAsync(request.UsuarioId, "AnularVenta", nameof(Venta), venta.Id.ToString(), $"Motivo: {request.Motivo}", ct: ct);

        await unitOfWork.GuardarCambiosAsync(ct);
        return await VentaResponseFactory.ConstruirAsync(venta, productos, ct);
    }
}
