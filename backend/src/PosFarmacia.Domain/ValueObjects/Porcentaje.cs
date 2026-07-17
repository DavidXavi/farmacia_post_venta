using PosFarmacia.Domain.Exceptions;

namespace PosFarmacia.Domain.ValueObjects;

public sealed record Porcentaje
{
    public static readonly Porcentaje Cero = new(0m);

    public decimal Valor { get; }

    public Porcentaje(decimal valor)
    {
        if (valor < 0 || valor > 100)
        {
            throw new ValorInvalidoException("El porcentaje debe estar entre 0 y 100.");
        }

        Valor = valor;
    }

    public decimal AplicarSobre(decimal monto) => Math.Round(monto * Valor / 100m, 2, MidpointRounding.AwayFromZero);

    public override string ToString() => $"{Valor}%";
}
