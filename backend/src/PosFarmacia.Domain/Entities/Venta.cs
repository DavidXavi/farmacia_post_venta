using PosFarmacia.Domain.Enums;
using PosFarmacia.Domain.Exceptions;
using PosFarmacia.Domain.ValueObjects;

namespace PosFarmacia.Domain.Entities;

public sealed class Venta : Entidad
{
    private readonly List<DetalleVenta> _detalles = [];
    private readonly List<Pago> _pagos = [];
    private readonly List<AplicacionPromocion> _aplicacionesPromocion = [];

    private Venta() { }

    public Venta(Guid cajaId, Guid sesionCajaId, Guid usuarioId, Guid? clienteId = null)
    {
        CajaId = cajaId;
        SesionCajaId = sesionCajaId;
        UsuarioId = usuarioId;
        ClienteId = clienteId;
        Fecha = DateTime.UtcNow;
        Estado = EstadoVenta.EnProceso;
    }

    public long? NumeroCorrelativo { get; private set; }

    public DateTime Fecha { get; private set; }

    public Guid CajaId { get; private set; }

    public Guid SesionCajaId { get; private set; }

    public Guid UsuarioId { get; private set; }

    public Guid? ClienteId { get; private set; }

    public Guid? ConvenioSeguroId { get; private set; }

    public Guid? LineaCreditoId { get; private set; }

    public EstadoVenta Estado { get; private set; }

    public IReadOnlyCollection<DetalleVenta> Detalles => _detalles;

    public IReadOnlyCollection<Pago> Pagos => _pagos;

    public IReadOnlyCollection<AplicacionPromocion> AplicacionesPromocion => _aplicacionesPromocion;

    public Comprobante? Comprobante { get; private set; }

    public Dinero Total => new(_detalles.Sum(d => d.Subtotal.Monto));

    public Dinero TotalPagado => new(_pagos.Sum(p => p.Monto.Monto));

    public DetalleVenta AgregarDetalle(Guid productoId, Cantidad cantidad, Dinero precioUnitario, Porcentaje tasaImpuesto, Guid? recetaId = null)
    {
        AsegurarEnProceso();
        var detalle = new DetalleVenta(Id, productoId, cantidad, precioUnitario, tasaImpuesto, recetaId);
        _detalles.Add(detalle);
        return detalle;
    }

    public void IdentificarCliente(Guid clienteId)
    {
        ClienteId = clienteId;
    }

    public void AplicarPromocionALinea(Guid detalleVentaId, Guid promocionId, Dinero montoDescuento, Porcentaje tasaImpuesto)
    {
        AsegurarEnProceso();

        if (_aplicacionesPromocion.Any(a => a.PromocionId == promocionId))
        {
            throw new PromocionInvalidaException("Esta promocion ya fue aplicada en otra linea de este comprobante.");
        }

        var detalle = _detalles.FirstOrDefault(d => d.Id == detalleVentaId)
            ?? throw new PromocionInvalidaException("La linea de venta indicada no existe en esta venta.");

        detalle.AplicarPromocion(promocionId, montoDescuento, tasaImpuesto);
        _aplicacionesPromocion.Add(new AplicacionPromocion(Id, detalleVentaId, promocionId, montoDescuento));
    }

    public void AsignarLoteADetalle(Guid detalleVentaId, Guid loteId, Cantidad cantidadTomada)
    {
        AsegurarEnProceso();
        var detalle = _detalles.FirstOrDefault(d => d.Id == detalleVentaId)
            ?? throw new StockInsuficienteException("La linea de venta indicada no existe en esta venta.");
        detalle.AsignarLote(loteId, cantidadTomada);
    }

    public void AsignarConvenio(Guid convenioSeguroId) => ConvenioSeguroId = convenioSeguroId;

    public void AsignarLineaCredito(Guid lineaCreditoId) => LineaCreditoId = lineaCreditoId;

    public Pago RegistrarPago(Guid formaPagoId, Dinero monto, string? codigoAutorizacion = null)
    {
        AsegurarEnProceso();
        var pago = new Pago(Id, formaPagoId, monto, codigoAutorizacion);
        _pagos.Add(pago);
        return pago;
    }

    public void Confirmar(long numeroCorrelativo, TipoComprobante tipoComprobante, string serieComprobante)
    {
        AsegurarEnProceso();

        if (_detalles.Count == 0)
        {
            throw new ValorInvalidoException("No se puede confirmar una venta sin productos.");
        }

        if (_detalles.Any(d => d.CantidadAsignadaEnLotes.Valor != d.Cantidad.Valor))
        {
            throw new StockInsuficienteException("Todas las lineas de venta deben tener lotes asignados por FEFO antes de confirmar.");
        }

        if (TotalPagado < Total)
        {
            throw new PagoInsuficienteException();
        }

        NumeroCorrelativo = numeroCorrelativo;
        Comprobante = new Comprobante(Id, tipoComprobante, new NumeroComprobante(serieComprobante, (int)numeroCorrelativo));
        Estado = EstadoVenta.Confirmada;
    }

    public bool EsDelMismoDia(DateOnly hoy) => DateOnly.FromDateTime(Fecha) == hoy;

    public void Anular(DateOnly hoy)
    {
        if (Estado != EstadoVenta.Confirmada)
        {
            throw new AnulacionNoPermitidaException("Solo una venta confirmada puede anularse.");
        }

        if (!EsDelMismoDia(hoy))
        {
            throw new AnulacionNoPermitidaException("La venta corresponde a un dia anterior; debe emitirse una nota de credito.");
        }

        Estado = EstadoVenta.Anulada;
    }

    private void AsegurarEnProceso()
    {
        if (Estado != EstadoVenta.EnProceso)
        {
            throw new VentaYaConfirmadaException();
        }
    }
}


