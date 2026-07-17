using PosFarmacia.Application.DTOs;
using PosFarmacia.Application.Mappers;
using PosFarmacia.Application.Ports;
using PosFarmacia.Domain.Entities;
using PosFarmacia.Domain.Enums;
using PosFarmacia.Domain.Exceptions;
using PosFarmacia.Domain.Repositories;

namespace PosFarmacia.Application.UseCases;

public sealed class EmitirNotaCreditoUseCase(
    IVentaRepository ventas,
    IUsuarioRepository usuarios,
    INotaCreditoRepository notasCredito,
    ReversionVentaService reversionVenta,
    IAuditService auditService,
    IUnitOfWork unitOfWork)
{
    public async Task<NotaCreditoResponse> EjecutarAsync(EmitirNotaCreditoRequest request, CancellationToken ct = default)
    {
        var venta = await ventas.ObtenerPorIdAsync(request.VentaId, ct)
            ?? throw new EntidadNoEncontradaException("La venta indicada no existe.");

        var usuario = await usuarios.ObtenerPorIdAsync(request.UsuarioId, ct)
            ?? throw new EntidadNoEncontradaException("El usuario indicado no existe.");

        if (!usuario.TienePermiso(PermisoEspecial.EmitirNotaCredito))
        {
            throw new AnulacionNoPermitidaException("El usuario no cuenta con permiso para emitir notas de credito.");
        }

        if (venta.Estado != EstadoVenta.Confirmada || venta.Comprobante is null)
        {
            throw new AnulacionNoPermitidaException("Solo se puede emitir una nota de credito sobre una venta confirmada.");
        }

        var notaCredito = new NotaCredito(venta.Id, venta.Comprobante.Id, request.UsuarioId, request.Motivo, venta.Total);
        await notasCredito.AgregarAsync(notaCredito, ct);

        await reversionVenta.RevertirStockAsync(venta, request.UsuarioId, ct);
        await reversionVenta.RevertirCreditoAsync(venta, ct);

        await auditService.RegistrarAsync(request.UsuarioId, "EmitirNotaCredito", nameof(NotaCredito), notaCredito.Id.ToString(),
            $"Venta {venta.Id}. Motivo: {request.Motivo}", ct: ct);

        await unitOfWork.GuardarCambiosAsync(ct);
        return notaCredito.ToResponse();
    }
}
