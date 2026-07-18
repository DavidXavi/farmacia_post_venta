namespace PosFarmacia.Domain.Entities;

public sealed class PromocionProducto
{
    private PromocionProducto() { }

    internal PromocionProducto(Guid promocionId, Guid productoId)
    {
        PromocionId = promocionId;
        ProductoId = productoId;
    }

    public Guid PromocionId { get; private set; }

    public Guid ProductoId { get; private set; }
}


