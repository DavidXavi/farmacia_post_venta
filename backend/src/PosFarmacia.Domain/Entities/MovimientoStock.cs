using PosFarmacia.Domain.Enums;
using PosFarmacia.Domain.ValueObjects;

namespace PosFarmacia.Domain.Entities;

public sealed class MovimientoStock : Entidad
{
    private MovimientoStock() { }

    public MovimientoStock(Guid loteId, TipoMovimientoStock tipo, Cantidad cantidad, Guid usuarioId, string? referencia = null)
    {
        LoteId = loteId;
        Tipo = tipo;
        Cantidad = cantidad;
        UsuarioId = usuarioId;
        Referencia = referencia;
        Fecha = DateTime.UtcNow;
    }

    public Guid LoteId { get; private set; }

    public TipoMovimientoStock Tipo { get; private set; }

    public Cantidad Cantidad { get; private set; } = Cantidad.Cero;

    public Guid UsuarioId { get; private set; }

    public string? Referencia { get; private set; }

    public DateTime Fecha { get; private set; }
}
