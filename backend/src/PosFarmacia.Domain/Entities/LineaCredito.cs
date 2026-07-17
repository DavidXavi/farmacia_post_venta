using PosFarmacia.Domain.Enums;
using PosFarmacia.Domain.Exceptions;
using PosFarmacia.Domain.ValueObjects;

namespace PosFarmacia.Domain.Entities;

public sealed class LineaCredito : Entidad
{
    private LineaCredito() { }

    public LineaCredito(Guid clienteId, Dinero montoAutorizado, DateOnly? vigenciaInicio, DateOnly? vigenciaFin)
    {
        ClienteId = clienteId;
        MontoAutorizado = montoAutorizado;
        SaldoDisponible = montoAutorizado;
        Vigencia = new PeriodoVigencia(vigenciaInicio, vigenciaFin);
        Estado = EstadoLineaCredito.Activa;
    }

    public Guid ClienteId { get; private set; }

    public Dinero MontoAutorizado { get; private set; } = Dinero.Cero;

    public Dinero SaldoDisponible { get; private set; } = Dinero.Cero;

    public PeriodoVigencia Vigencia { get; private set; } = null!;

    public EstadoLineaCredito Estado { get; private set; }

    public bool EstaActivaYVigente(DateOnly hoy) => Estado == EstadoLineaCredito.Activa && Vigencia.EstaVigente(hoy);

    public void Consumir(Dinero monto)
    {
        if (monto > SaldoDisponible)
        {
            throw new LineaCreditoInvalidaException("El importe financiado supera el saldo disponible del cliente.");
        }

        SaldoDisponible -= monto;
    }

    public void Revertir(Dinero monto) => SaldoDisponible = new Dinero(Math.Min(MontoAutorizado.Monto, SaldoDisponible.Monto + monto.Monto));

    public void Bloquear() => Estado = EstadoLineaCredito.Bloqueada;
}
