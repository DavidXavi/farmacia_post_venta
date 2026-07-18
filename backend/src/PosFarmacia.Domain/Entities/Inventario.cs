using PosFarmacia.Domain.ValueObjects;

namespace PosFarmacia.Domain.Entities;

/// Rollup de stock actual por producto+local, recalculado desde los lotes (nunca es fuente de verdad).
public sealed class Inventario : Entidad
{
    private Inventario() { }

    public Inventario(Guid productoId, Guid localId, Cantidad cantidadActual)
    {
        ProductoId = productoId;
        LocalId = localId;
        CantidadActual = cantidadActual;
        ActualizadoEn = DateTime.UtcNow;
    }

    public Guid ProductoId { get; private set; }

    public Guid LocalId { get; private set; }

    public Cantidad CantidadActual { get; private set; } = Cantidad.Cero;

    public DateTime ActualizadoEn { get; private set; }

    public void Actualizar(Cantidad cantidadActual)
    {
        CantidadActual = cantidadActual;
        ActualizadoEn = DateTime.UtcNow;
    }
}
