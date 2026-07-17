using PosFarmacia.Application.DTOs;
using PosFarmacia.Domain.Entities;
using PosFarmacia.Domain.Exceptions;
using PosFarmacia.Domain.Repositories;

namespace PosFarmacia.Application.UseCases;

public sealed class IniciarVentaUseCase(
    ISesionCajaRepository sesiones,
    IClienteRepository clientes,
    IVentaRepository ventas,
    IProductoRepository productos,
    IUnitOfWork unitOfWork)
{
    public async Task<VentaResponse> EjecutarAsync(IniciarVentaRequest request, CancellationToken ct = default)
    {
        var sesion = await sesiones.ObtenerPorIdAsync(request.SesionCajaId, ct)
            ?? throw new EntidadNoEncontradaException("La sesion de caja indicada no existe.");
        sesion.AsegurarAbierta();

        Guid? clienteId = null;
        if (!string.IsNullOrWhiteSpace(request.ClienteDni))
        {
            var cliente = await clientes.ObtenerPorDniAsync(request.ClienteDni, ct)
                ?? throw new EntidadNoEncontradaException("No existe un cliente registrado con ese DNI.");
            clienteId = cliente.Id;
        }

        var venta = new Venta(request.CajaId, request.SesionCajaId, request.UsuarioId, clienteId);
        await ventas.AgregarAsync(venta, ct);
        await unitOfWork.GuardarCambiosAsync(ct);

        return await VentaResponseFactory.ConstruirAsync(venta, productos, ct);
    }
}
