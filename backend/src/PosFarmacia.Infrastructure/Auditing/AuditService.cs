using PosFarmacia.Application.Ports;
using PosFarmacia.Domain.Entities;
using PosFarmacia.Infrastructure.Persistence;

namespace PosFarmacia.Infrastructure.Auditing;

/// No llama SaveChanges: queda dentro de la misma transaccion que confirma la operacion que audita (RN06).
public sealed class AuditService(PosFarmaciaDbContext contexto) : IAuditService
{
    public async Task RegistrarAsync(Guid usuarioId, string accion, string entidad, string entidadId, string detalle,
        string? datosAnteriores = null, string? datosNuevos = null, CancellationToken ct = default)
    {
        var registro = new Auditoria(usuarioId, accion, entidad, entidadId, detalle, datosAnteriores, datosNuevos);
        await contexto.Auditorias.AddAsync(registro, ct);
    }
}
