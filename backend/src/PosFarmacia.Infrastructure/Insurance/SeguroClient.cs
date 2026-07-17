using Microsoft.EntityFrameworkCore;
using PosFarmacia.Application.Ports;
using PosFarmacia.Domain.Entities;
using PosFarmacia.Domain.Enums;
using PosFarmacia.Infrastructure.Persistence;

namespace PosFarmacia.Infrastructure.Insurance;

/// ponytail: simulador in-proceso del servicio central de seguros (permitido por el enunciado, seccion 12).
/// Para reemplazarlo por un proveedor real solo hace falta otra implementacion de ISeguroClient.
public sealed class SeguroClient(PosFarmaciaDbContext contexto) : ISeguroClient
{
    public async Task<ConvenioSeguro?> ConsultarConvenioVigenteAsync(Guid clienteId, Guid convenioId, DateOnly hoy, CancellationToken ct = default)
    {
        var afiliacionActiva = await contexto.AfiliacionesCliente
            .AnyAsync(a => a.ClienteId == clienteId && a.ConvenioId == convenioId && a.Estado == EstadoAfiliacion.Activa, ct);

        if (!afiliacionActiva)
        {
            return null;
        }

        var convenio = await contexto.ConveniosSeguro.Include(c => c.Coberturas).FirstOrDefaultAsync(c => c.Id == convenioId, ct);
        if (convenio is null || !convenio.Activo)
        {
            return null;
        }

        var afiliacion = await contexto.AfiliacionesCliente
            .FirstAsync(a => a.ClienteId == clienteId && a.ConvenioId == convenioId, ct);

        return afiliacion.EstaActivaYVigente(hoy) ? convenio : null;
    }
}
