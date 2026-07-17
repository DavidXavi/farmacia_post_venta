using PosFarmacia.Domain.Entities;
using PosFarmacia.Domain.Enums;
using PosFarmacia.Domain.Exceptions;

namespace PosFarmacia.Domain.Services;

public sealed class ValidadorReceta
{
    public void ValidarParaDispensacion(Receta receta, Guid productoId, DateOnly hoy)
    {
        if (receta.ProductoId != productoId)
        {
            throw new RecetaInvalidaException("La receta no corresponde al medicamento que se desea dispensar.");
        }

        if (receta.Estado != EstadoReceta.Aprobada)
        {
            throw new RecetaInvalidaException("La receta debe estar aprobada por el quimico farmaceutico antes de dispensar.");
        }

        if (receta.EstaVencida(hoy))
        {
            throw new RecetaInvalidaException("La receta se encuentra vencida.");
        }

        if (!receta.PuedeUsarseNuevamente())
        {
            throw new RecetaYaUtilizadaException();
        }

        if (receta.Tipo != TipoReceta.Normal && receta.FechaVencimiento is null)
        {
            throw new RecetaInvalidaException("Las recetas especiales requieren fecha de vencimiento.");
        }
    }
}
