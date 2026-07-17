using PosFarmacia.Domain.Enums;

namespace PosFarmacia.Domain.Entities;

public sealed class ValidacionReceta : Entidad
{
    private ValidacionReceta() { }

    public ValidacionReceta(Guid recetaId, Guid usuarioValidadorId, EstadoReceta resultado, string? observaciones)
    {
        RecetaId = recetaId;
        UsuarioValidadorId = usuarioValidadorId;
        Resultado = resultado;
        Observaciones = observaciones;
        Fecha = DateTime.UtcNow;
    }

    public Guid RecetaId { get; private set; }

    public Guid UsuarioValidadorId { get; private set; }

    public DateTime Fecha { get; private set; }

    public EstadoReceta Resultado { get; private set; }

    public string? Observaciones { get; private set; }
}
