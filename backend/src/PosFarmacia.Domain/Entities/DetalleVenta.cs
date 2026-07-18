using PosFarmacia.Domain.Exceptions;
using PosFarmacia.Domain.ValueObjects;

namespace PosFarmacia.Domain.Entities;

public sealed class DetalleVenta : Entidad
{
    private readonly List<DetalleVentaLote> _lotes = [];

    private DetalleVenta() { }

    public DetalleVenta(Guid ventaId, Guid productoId, Cantidad cantidad, Dinero precioUnitario, Porcentaje tasaImpuesto, Guid? recetaId = null)
    {
        VentaId = ventaId;
        ProductoId = productoId;
        Cantidad = cantidad;
        PrecioUnitario = precioUnitario;
        DescuentoMonto = Dinero.Cero;
        RecetaId = recetaId;
        RecalcularImpuestoYSubtotal(tasaImpuesto);
    }

    public Guid VentaId { get; private set; }

    public Guid ProductoId { get; private set; }

    public Cantidad Cantidad { get; private set; } = Cantidad.Cero;

    public Dinero PrecioUnitario { get; private set; } = Dinero.Cero;

    public Guid? PromocionAplicadaId { get; private set; }

    public Guid? RecetaId { get; private set; }

    public Dinero DescuentoMonto { get; private set; } = Dinero.Cero;

    public Dinero ImpuestoMonto { get; private set; } = Dinero.Cero;

    public Dinero Subtotal { get; private set; } = Dinero.Cero;

    public IReadOnlyCollection<DetalleVentaLote> Lotes => _lotes;

    public Cantidad CantidadAsignadaEnLotes => new(_lotes.Sum(l => l.CantidadTomada.Valor));

    public void AplicarPromocion(Guid promocionId, Dinero montoDescuento, Porcentaje tasaImpuesto)
    {
        if (PromocionAplicadaId is not null)
        {
            throw new PromocionInvalidaException("Esta linea de venta ya tiene una promocion aplicada.");
        }

        PromocionAplicadaId = promocionId;
        DescuentoMonto = montoDescuento;
        RecalcularImpuestoYSubtotal(tasaImpuesto);
    }

    public void AsignarLote(Guid loteId, Cantidad cantidadTomada)
    {
        if (CantidadAsignadaEnLotes + cantidadTomada > Cantidad)
        {
            throw new StockInsuficienteException("La cantidad asignada de lotes excede la cantidad vendida en la linea.");
        }

        _lotes.Add(new DetalleVentaLote(Id, loteId, cantidadTomada));
    }

    private void RecalcularImpuestoYSubtotal(Porcentaje tasaImpuesto)
    {
        var baseImponible = new Dinero((PrecioUnitario.Monto * Cantidad.Valor) - DescuentoMonto.Monto);
        ImpuestoMonto = new Dinero(tasaImpuesto.AplicarSobre(baseImponible.Monto));
        Subtotal = baseImponible + ImpuestoMonto;
    }
}


