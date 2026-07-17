namespace PosFarmacia.Domain.Entities;

public sealed class Laboratorio : Entidad
{
    private Laboratorio() { }

    public Laboratorio(string nombre)
    {
        Nombre = nombre;
    }

    public string Nombre { get; private set; } = string.Empty;
}
