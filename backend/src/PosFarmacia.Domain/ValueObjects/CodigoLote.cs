using PosFarmacia.Domain.Exceptions;

namespace PosFarmacia.Domain.ValueObjects;

public sealed record CodigoLote
{
    public string Valor { get; }

    public CodigoLote(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
        {
            throw new ValorInvalidoException("El codigo de lote no puede ser vacio.");
        }

        Valor = valor.Trim();
    }

    public override string ToString() => Valor;
}
