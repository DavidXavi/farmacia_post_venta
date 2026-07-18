namespace PosFarmacia.Domain.Entities;

public sealed class Auditoria : Entidad
{
    private Auditoria() { }

    public Auditoria(Guid usuarioId, string accion, string entidad, string entidadId, string detalle,
        string? datosAnteriores = null, string? datosNuevos = null)
    {
        UsuarioId = usuarioId;
        Accion = accion;
        Entidad = entidad;
        EntidadId = entidadId;
        Detalle = detalle;
        DatosAnteriores = datosAnteriores;
        DatosNuevos = datosNuevos;
        Fecha = DateTime.UtcNow;
    }

    public DateTime Fecha { get; private set; }

    public Guid UsuarioId { get; private set; }

    public string Accion { get; private set; } = string.Empty;

    public string Entidad { get; private set; } = string.Empty;

    public string EntidadId { get; private set; } = string.Empty;

    public string Detalle { get; private set; } = string.Empty;

    public string? DatosAnteriores { get; private set; }

    public string? DatosNuevos { get; private set; }
}


