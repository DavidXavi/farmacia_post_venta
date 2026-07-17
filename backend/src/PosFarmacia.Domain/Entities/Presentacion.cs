namespace PosFarmacia.Domain.Entities;

public sealed class Presentacion : Entidad
{
    private Presentacion() { }

    public Presentacion(string nombre, string unidadMedida)
    {
        Nombre = nombre;
        UnidadMedida = unidadMedida;
    }

    public string Nombre { get; private set; } = string.Empty;

    public string UnidadMedida { get; private set; } = string.Empty;
}
