using PosFarmacia.Domain.Exceptions;

namespace PosFarmacia.Domain.ValueObjects;

public sealed record Dinero
{
    public static readonly Dinero Cero = new(0m);

    public decimal Monto { get; }

    public Dinero(decimal monto)
    {
        if (monto < 0)
        {
            throw new ValorInvalidoException("El monto de dinero no puede ser negativo.");
        }

        Monto = Math.Round(monto, 2, MidpointRounding.AwayFromZero);
    }

    public static Dinero operator +(Dinero a, Dinero b) => new(a.Monto + b.Monto);

    public static Dinero operator -(Dinero a, Dinero b) => new(a.Monto - b.Monto);

    public static Dinero operator *(Dinero a, decimal factor) => new(a.Monto * factor);

    public static bool operator <(Dinero a, Dinero b) => a.Monto < b.Monto;

    public static bool operator >(Dinero a, Dinero b) => a.Monto > b.Monto;

    public static bool operator <=(Dinero a, Dinero b) => a.Monto <= b.Monto;

    public static bool operator >=(Dinero a, Dinero b) => a.Monto >= b.Monto;

    public override string ToString() => Monto.ToString("F2");
}
