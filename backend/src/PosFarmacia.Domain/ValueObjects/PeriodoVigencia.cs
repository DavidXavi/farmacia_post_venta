using PosFarmacia.Domain.Exceptions;

namespace PosFarmacia.Domain.ValueObjects;

public sealed record PeriodoVigencia
{
    public DateOnly? Inicio { get; }

    public DateOnly? Fin { get; }

    public PeriodoVigencia(DateOnly? inicio, DateOnly? fin)
    {
        if (inicio is not null && fin is not null && fin < inicio)
        {
            throw new ValorInvalidoException("La fecha de fin de vigencia no puede ser anterior a la de inicio.");
        }

        Inicio = inicio;
        Fin = fin;
    }

    public bool EstaVigente(DateOnly hoy) =>
        (Inicio is null || Inicio <= hoy) && (Fin is null || Fin >= hoy);
}
