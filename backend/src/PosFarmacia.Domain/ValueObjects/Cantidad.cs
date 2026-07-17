using PosFarmacia.Domain.Exceptions;

namespace PosFarmacia.Domain.ValueObjects;

public sealed record Cantidad
{
    public static readonly Cantidad Cero = new(0);

    public int Valor { get; }

    public Cantidad(int valor)
    {
        if (valor < 0)
        {
            throw new ValorInvalidoException("La cantidad no puede ser negativa.");
        }

        Valor = valor;
    }

    public static Cantidad operator +(Cantidad a, Cantidad b) => new(a.Valor + b.Valor);

    public static Cantidad operator -(Cantidad a, Cantidad b) => new(a.Valor - b.Valor);

    public static bool operator >(Cantidad a, Cantidad b) => a.Valor > b.Valor;

    public static bool operator <(Cantidad a, Cantidad b) => a.Valor < b.Valor;

    public static bool operator >=(Cantidad a, Cantidad b) => a.Valor >= b.Valor;

    public static bool operator <=(Cantidad a, Cantidad b) => a.Valor <= b.Valor;

    public override string ToString() => Valor.ToString();
}
