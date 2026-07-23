using PosFarmacia.Application.DTOs;
using PosFarmacia.Application.Mappers;
using PosFarmacia.Domain.Entities;
using PosFarmacia.Domain.Exceptions;
using PosFarmacia.Domain.Repositories;
using PosFarmacia.Domain.ValueObjects;

namespace PosFarmacia.Application.UseCases;

public sealed class RegistrarClienteUseCase(IClienteRepository clientes, IUnitOfWork unitOfWork)
{
    public async Task<ClienteResponse> EjecutarAsync(RegistrarClienteRequest request, CancellationToken ct = default)
    {
        var dni = new Dni(request.Dni);
        if (await clientes.ObtenerPorDniAsync(dni.Valor, ct) is not null)
        {
            throw new ValorInvalidoException("Ya existe un cliente registrado con ese DNI.");
        }

        var cliente = new Cliente(dni, request.Nombres, request.Apellidos, request.FechaNacimiento,
            request.Telefono, request.Correo, request.Direccion);

        await clientes.AgregarAsync(cliente, ct);
        await unitOfWork.GuardarCambiosAsync(ct);
        return cliente.ToResponse();
    }
}

public sealed class BuscarClientePorDniUseCase(IClienteRepository clientes)
{
    public async Task<ClienteResponse?> EjecutarAsync(string dni, CancellationToken ct = default)
    {
        var cliente = await clientes.ObtenerPorDniAsync(dni, ct);
        return cliente?.ToResponse();
    }
}

public sealed class ConsultarClientesUseCase(IClienteRepository clientes)
{
    public async Task<IReadOnlyList<ClienteResponse>> EjecutarAsync(CancellationToken ct = default)
    {
        var resultado = await clientes.ObtenerTodosAsync(ct);
        return resultado.Select(c => c.ToResponse()).ToList();
    }
}

public sealed class ActualizarClienteUseCase(IClienteRepository clientes, IUnitOfWork unitOfWork)
{
    public async Task<ClienteResponse> EjecutarAsync(Guid clienteId, ActualizarClienteRequest request, CancellationToken ct = default)
    {
        var cliente = await clientes.ObtenerPorIdAsync(clienteId, ct)
            ?? throw new EntidadNoEncontradaException("El cliente indicado no existe.");

        cliente.ActualizarDatos(request.Telefono, request.Correo, request.Direccion);
        await unitOfWork.GuardarCambiosAsync(ct);
        return cliente.ToResponse();
    }
}
