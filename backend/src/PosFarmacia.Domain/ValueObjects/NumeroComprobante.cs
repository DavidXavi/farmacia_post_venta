using PosFarmacia.Domain.Exceptions;

namespace PosFarmacia.Domain.ValueObjects;

public sealed record NumeroComprobante
{
    public string Serie { get; }

    public int Correlativo { get; }

    public NumeroComprobante(string serie, int correlativo)
    {
        if (string.IsNullOrWhiteSpace(serie))
        {
            throw new ValorInvalidoException("La serie del comprobante no puede ser vacia.");
        }

        if (correlativo <= 0)
        {
            throw new ValorInvalidoException("El correlativo del comprobante debe ser mayor a cero.");
        }

        Serie = serie.Trim();
        Correlativo = correlativo;
    }

    public override string ToString() => $"{Serie}-{Correlativo:D8}";
}
