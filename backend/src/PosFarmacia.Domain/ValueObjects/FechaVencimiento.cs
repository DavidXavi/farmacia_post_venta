namespace PosFarmacia.Domain.ValueObjects;

public sealed record FechaVencimiento
{
    public DateOnly Valor { get; }

    public FechaVencimiento(DateOnly valor)
    {
        Valor = valor;
    }

    public bool EstaVencida(DateOnly hoy) => Valor < hoy;

    public bool EstaEnPeriodoPreventivo(DateOnly hoy, int mesesPreventivos = 3) =>
        !EstaVencida(hoy) && Valor <= hoy.AddMonths(mesesPreventivos);

    public override string ToString() => Valor.ToString("yyyy-MM-dd");
}
