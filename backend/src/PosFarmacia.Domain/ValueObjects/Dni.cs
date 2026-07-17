using System.Text.RegularExpressions;
using PosFarmacia.Domain.Exceptions;

namespace PosFarmacia.Domain.ValueObjects;

public sealed partial record Dni
{
    public string Valor { get; }

    public Dni(string valor)
    {
        if (!DniRegex().IsMatch(valor ?? string.Empty))
        {
            throw new ValorInvalidoException("El DNI debe tener exactamente 8 digitos numericos.");
        }

        Valor = valor!;
    }

    public override string ToString() => Valor;

    [GeneratedRegex(@"^\d{8}$")]
    private static partial Regex DniRegex();
}
