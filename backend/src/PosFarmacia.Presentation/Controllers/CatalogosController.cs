using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PosFarmacia.Application.DTOs;
using PosFarmacia.Application.UseCases;

namespace PosFarmacia.Presentation.Controllers;

[ApiController]
[Route("api/categorias")]
[Authorize]
public sealed class CategoriasController(RegistrarCategoriaUseCase registrar, ConsultarCategoriasUseCase consultar) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<ActionResult<CategoriaResponse>> Registrar(NombreRequest request, CancellationToken ct) =>
        Ok(await registrar.EjecutarAsync(request, ct));

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CategoriaResponse>>> Listar(CancellationToken ct) => Ok(await consultar.EjecutarAsync(ct));
}

[ApiController]
[Route("api/laboratorios")]
[Authorize]
public sealed class LaboratoriosController(RegistrarLaboratorioUseCase registrar, ConsultarLaboratoriosUseCase consultar) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<ActionResult<LaboratorioResponse>> Registrar(NombreRequest request, CancellationToken ct) =>
        Ok(await registrar.EjecutarAsync(request, ct));

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<LaboratorioResponse>>> Listar(CancellationToken ct) => Ok(await consultar.EjecutarAsync(ct));
}

[ApiController]
[Route("api/presentaciones")]
[Authorize]
public sealed class PresentacionesController(RegistrarPresentacionUseCase registrar, ConsultarPresentacionesUseCase consultar) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<ActionResult<PresentacionResponse>> Registrar(RegistrarPresentacionRequest request, CancellationToken ct) =>
        Ok(await registrar.EjecutarAsync(request, ct));

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<PresentacionResponse>>> Listar(CancellationToken ct) => Ok(await consultar.EjecutarAsync(ct));
}

[ApiController]
[Route("api/formas-pago")]
[Authorize]
public sealed class FormasPagoController(RegistrarFormaPagoUseCase registrar, ConsultarFormasPagoUseCase consultar) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<ActionResult<FormaPagoResponse>> Registrar(RegistrarFormaPagoRequest request, CancellationToken ct) =>
        Ok(await registrar.EjecutarAsync(request, ct));

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<FormaPagoResponse>>> Listar(CancellationToken ct) => Ok(await consultar.EjecutarAsync(ct));
}

[ApiController]
[Route("api/locales")]
[Authorize]
public sealed class LocalesController(RegistrarLocalUseCase registrar, ConsultarLocalesUseCase consultar) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<ActionResult<LocalResponse>> Registrar(RegistrarLocalRequest request, CancellationToken ct) =>
        Ok(await registrar.EjecutarAsync(request, ct));

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<LocalResponse>>> Listar(CancellationToken ct) => Ok(await consultar.EjecutarAsync(ct));
}

[ApiController]
[Route("api/reglas-incentivo")]
[Authorize(Roles = "Administrador")]
public sealed class ReglasIncentivoController(RegistrarReglaIncentivoUseCase registrar, ConsultarReglasIncentivoUseCase consultar) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<ReglaIncentivoResponse>> Registrar(RegistrarReglaIncentivoRequest request, CancellationToken ct) =>
        Ok(await registrar.EjecutarAsync(request, ct));

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ReglaIncentivoResponse>>> Listar(CancellationToken ct) => Ok(await consultar.EjecutarAsync(ct));
}
