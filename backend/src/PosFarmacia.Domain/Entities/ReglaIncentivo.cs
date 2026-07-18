using PosFarmacia.Domain.ValueObjects;

namespace PosFarmacia.Domain.Entities;

public sealed class ReglaIncentivo : Entidad
{
    private ReglaIncentivo() { }

    public ReglaIncentivo(string nombre, Guid? productoId, Guid? categoriaId, Dinero montoPorUnidad,
        DateOnly fechaInicio, DateOnly fechaFin)
    {
        Nombre = nombre;
        ProductoId = productoId;
        CategoriaId = categoriaId;
        MontoPorUnidad = montoPorUnidad;
        Vigencia = new PeriodoVigencia(fechaInicio, fechaFin);
        Activa = true;
    }

    public string Nombre { get; private set; } = string.Empty;

    public Guid? ProductoId { get; private set; }

    public Guid? CategoriaId { get; private set; }

    public Dinero MontoPorUnidad { get; private set; } = Dinero.Cero;

    public PeriodoVigencia Vigencia { get; private set; } = null!;

    public bool Activa { get; private set; }

    public bool AplicaA(Guid productoId, Guid categoriaId, DateOnly hoy) =>
        Activa && Vigencia.EstaVigente(hoy) && (ProductoId == productoId || CategoriaId == categoriaId);

    public void Desactivar() => Activa = false;
}


