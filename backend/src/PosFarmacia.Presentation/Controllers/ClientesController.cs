using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PosFarmacia.Application.DTOs;
using PosFarmacia.Application.UseCases;

namespace PosFarmacia.Presentation.Controllers;

[ApiController]
[Route("api/clientes")]
[Authorize]
public sealed class ClientesController(
    RegistrarClienteUseCase registrarCliente,
    BuscarClientePorDniUseCase buscarClientePorDni,
    ConsultarClientesUseCase consultarClientes,
    ActualizarClienteUseCase actualizarCliente,
    ConsultarConvenioDeClienteUseCase consultarConvenios,
    ConsultarLineaCreditoUseCase consultarLineaCredito) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<ClienteResponse>> Registrar(RegistrarClienteRequest request, CancellationToken ct) =>
        Ok(await registrarCliente.EjecutarAsync(request, ct));

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ClienteResponse>>> Listar(CancellationToken ct) =>
        Ok(await consultarClientes.EjecutarAsync(ct));

    [HttpPatch("{id:guid}")]
    public async Task<ActionResult<ClienteResponse>> Actualizar(Guid id, ActualizarClienteRequest request, CancellationToken ct) =>
        Ok(await actualizarCliente.EjecutarAsync(id, request, ct));

    [HttpGet("dni/{dni}")]
    public async Task<ActionResult<ClienteResponse>> BuscarPorDni(string dni, CancellationToken ct)
    {
        var cliente = await buscarClientePorDni.EjecutarAsync(dni, ct);
        return cliente is null ? NotFound() : Ok(cliente);
    }

    [HttpGet("{id:guid}/convenios")]
    public async Task<ActionResult<IReadOnlyList<AfiliacionResponse>>> Convenios(Guid id, CancellationToken ct) =>
        Ok(await consultarConvenios.EjecutarAsync(id, ct));

    [HttpGet("{id:guid}/linea-credito")]
    public async Task<ActionResult<LineaCreditoResponse>> LineaCredito(Guid id, CancellationToken ct)
    {
        var linea = await consultarLineaCredito.EjecutarAsync(id, ct);
        return linea is null ? NotFound() : Ok(linea);
    }
}


