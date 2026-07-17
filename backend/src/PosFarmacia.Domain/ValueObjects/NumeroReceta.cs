using PosFarmacia.Domain.Exceptions;

namespace PosFarmacia.Domain.ValueObjects;

public sealed record NumeroReceta
{
    public string Valor { get; }

    public NumeroReceta(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
        {
            throw new ValorInvalidoException("El numero de receta no puede ser vacio.");
        }

        Valor = valor.Trim();
    }

    public override string ToString() => Valor;
}
