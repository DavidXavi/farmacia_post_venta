using PosFarmacia.Domain.Exceptions;

namespace PosFarmacia.Domain.ValueObjects;

public sealed record CodigoBarras
{
    public string Valor { get; }

    public CodigoBarras(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
        {
            throw new ValorInvalidoException("El codigo de barras no puede ser vacio.");
        }

        Valor = valor.Trim();
    }

    public override string ToString() => Valor;
}
