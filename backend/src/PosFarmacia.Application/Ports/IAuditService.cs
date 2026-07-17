namespace PosFarmacia.Application.Ports;

public interface IAuditService
{
    Task RegistrarAsync(Guid usuarioId, string accion, string entidad, string entidadId, string detalle,
        string? datosAnteriores = null, string? datosNuevos = null, CancellationToken ct = default);
}
