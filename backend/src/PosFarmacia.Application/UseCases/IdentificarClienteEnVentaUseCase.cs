using PosFarmacia.Application.DTOs;
using PosFarmacia.Domain.Exceptions;
using PosFarmacia.Domain.Repositories;

namespace PosFarmacia.Application.UseCases;

public sealed class IdentificarClienteEnVentaUseCase(
    IVentaRepository ventas,
    IClienteRepository clientes,
    IProductoRepository productos,
    IUnitOfWork unitOfWork)
{
    public async Task<VentaResponse> EjecutarAsync(Guid ventaId, IdentificarClienteRequest request, CancellationToken ct = default)
    {
        var venta = await ventas.ObtenerPorIdAsync(ventaId, ct)
            ?? throw new EntidadNoEncontradaException("La venta indicada no existe.");

        var cliente = await clientes.ObtenerPorDniAsync(request.Dni, ct)
            ?? throw new EntidadNoEncontradaException("No existe un cliente registrado con ese DNI.");

        venta.IdentificarCliente(cliente.Id);
        await unitOfWork.GuardarCambiosAsync(ct);

        return await VentaResponseFactory.ConstruirAsync(venta, productos, ct);
    }
}
