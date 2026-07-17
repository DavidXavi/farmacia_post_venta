namespace PosFarmacia.Domain.Entities;

public sealed class Categoria : Entidad
{
    private Categoria() { }

    public Categoria(string nombre)
    {
        Nombre = nombre;
    }

    public string Nombre { get; private set; } = string.Empty;
}
