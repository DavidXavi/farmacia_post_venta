using PosFarmacia.Application.DTOs;
using PosFarmacia.Domain.Entities;

namespace PosFarmacia.Application.Mappers;

public static class NotaCreditoYAuditoriaMappers
{
    public static NotaCreditoResponse ToResponse(this NotaCredito n) => new(
        n.Id, n.VentaId, n.ComprobanteId, n.UsuarioId, n.Motivo, n.MontoTotal.Monto, n.Fecha);

    public static AuditoriaResponse ToResponse(this Auditoria a) => new(
        a.Id, a.Fecha, a.UsuarioId, a.Accion, a.Entidad, a.EntidadId, a.Detalle);
}
