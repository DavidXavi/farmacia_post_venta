using PosFarmacia.Domain.Enums;
using PosFarmacia.Domain.Exceptions;
using PosFarmacia.Domain.ValueObjects;

namespace PosFarmacia.Domain.Entities;

public sealed class Lote : Entidad
{
    private Lote() { }

    public Lote(CodigoLote codigo, Guid productoId, FechaVencimiento fechaVencimiento, Cantidad cantidadRecibida,
        Guid localId, Dinero? costo = null)
    {
        Codigo = codigo;
        ProductoId = productoId;
        FechaVencimiento = fechaVencimiento;
        CantidadRecibida = cantidadRecibida;
        CantidadDisponible = cantidadRecibida;
        LocalId = localId;
        Costo = costo;
        Estado = EstadoLote.Disponible;
    }

    public CodigoLote Codigo { get; private set; } = null!;

    public Guid ProductoId { get; private set; }

    public FechaVencimiento FechaVencimiento { get; private set; } = null!;

    public Cantidad CantidadRecibida { get; private set; } = Cantidad.Cero;

    public Cantidad CantidadDisponible { get; private set; } = Cantidad.Cero;

    public Dinero? Costo { get; private set; }

    public Guid LocalId { get; private set; }

    public EstadoLote Estado { get; private set; }

    public bool EsVendible(DateOnly hoy, int mesesPreventivos = 3) =>
        Estado == EstadoLote.Disponible
        && CantidadDisponible.Valor > 0
        && !FechaVencimiento.EstaVencida(hoy)
        && !FechaVencimiento.EstaEnPeriodoPreventivo(hoy, mesesPreventivos);

    public void Reservar(Cantidad cantidad)
    {
        if (cantidad > CantidadDisponible)
        {
            throw new StockInsuficienteException($"El lote {Codigo} no tiene stock suficiente.");
        }

        CantidadDisponible -= cantidad;
        if (CantidadDisponible.Valor == 0)
        {
            Estado = EstadoLote.Agotado;
        }
    }

    /// RN43: un lote vencido, retirado o bloqueado no vuelve a ser stock vendible aunque se reverse la venta.
    /// Devuelve false cuando la devolucion no pudo aplicarse por ese motivo.
    public bool Devolver(Cantidad cantidad)
    {
        if (Estado is EstadoLote.Vencido or EstadoLote.Retirado or EstadoLote.Bloqueado)
        {
            return false;
        }

        CantidadDisponible += cantidad;
        Estado = EstadoLote.Disponible;
        return true;
    }

    public void Bloquear() => Estado = EstadoLote.Bloqueado;

    public void Retirar() => Estado = EstadoLote.Retirado;

    public void MarcarVencido() => Estado = EstadoLote.Vencido;
}
