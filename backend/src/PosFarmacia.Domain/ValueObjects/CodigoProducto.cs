using PosFarmacia.Domain.Exceptions;

namespace PosFarmacia.Domain.ValueObjects;

public sealed record CodigoProducto
{
    public string Valor { get; }

    public CodigoProducto(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
        {
            throw new ValorInvalidoException("El codigo de producto no puede ser vacio.");
        }

        Valor = valor.Trim();
    }

    public override string ToString() => Valor;
}
