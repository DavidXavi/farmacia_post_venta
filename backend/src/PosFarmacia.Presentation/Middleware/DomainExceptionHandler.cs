using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using PosFarmacia.Domain.Exceptions;

namespace PosFarmacia.Presentation.Middleware;

public sealed class DomainExceptionHandler(ILogger<DomainExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken ct)
    {
        var (status, titulo) = exception switch
        {
            EntidadNoEncontradaException => (StatusCodes.Status404NotFound, exception.Message),
            ValorInvalidoException => (StatusCodes.Status400BadRequest, exception.Message),
            CajaCerradaException => (StatusCodes.Status409Conflict, exception.Message),
            StockInsuficienteException => (StatusCodes.Status409Conflict, exception.Message),
            VentaYaConfirmadaException => (StatusCodes.Status409Conflict, exception.Message),
            AnulacionNoPermitidaException => (StatusCodes.Status409Conflict, exception.Message),
            DbUpdateConcurrencyException => (StatusCodes.Status409Conflict, "Otro usuario modifico este registro al mismo tiempo. Intente nuevamente."),
            PagoInsuficienteException => (StatusCodes.Status422UnprocessableEntity, exception.Message),
            PromocionInvalidaException => (StatusCodes.Status422UnprocessableEntity, exception.Message),
            RecetaInvalidaException => (StatusCodes.Status422UnprocessableEntity, exception.Message),
            RecetaYaUtilizadaException => (StatusCodes.Status422UnprocessableEntity, exception.Message),
            ConvenioNoDisponibleException => (StatusCodes.Status422UnprocessableEntity, exception.Message),
            LineaCreditoInvalidaException => (StatusCodes.Status422UnprocessableEntity, exception.Message),
            DomainException => (StatusCodes.Status400BadRequest, exception.Message),
            _ => (StatusCodes.Status500InternalServerError, "Ocurrio un error inesperado.")
        };

        if (status == StatusCodes.Status500InternalServerError)
        {
            logger.LogError(exception, "Error no controlado procesando {Path}", httpContext.Request.Path);
        }

        httpContext.Response.StatusCode = status;
        await httpContext.Response.WriteAsJsonAsync(new { title = titulo, status }, ct);
        return true;
    }
}
