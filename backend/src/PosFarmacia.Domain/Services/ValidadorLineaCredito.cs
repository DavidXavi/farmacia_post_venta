using PosFarmacia.Domain.Entities;
using PosFarmacia.Domain.Exceptions;
using PosFarmacia.Domain.ValueObjects;

namespace PosFarmacia.Domain.Services;

public sealed class ValidadorLineaCredito
{
    public void ValidarParaConsumo(LineaCredito lineaCredito, Dinero monto, DateOnly hoy)
    {
        if (!lineaCredito.EstaActivaYVigente(hoy))
        {
            throw new LineaCreditoInvalidaException("La linea de credito no esta activa o vigente.");
        }

        if (monto > lineaCredito.SaldoDisponible)
        {
            throw new LineaCreditoInvalidaException("El monto solicitado supera el saldo disponible del cliente.");
        }
    }
}
