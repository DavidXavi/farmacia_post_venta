namespace PosFarmacia.Domain.Entities;

public sealed class Caja : Entidad
{
    private Caja() { }

    public Caja(string nombre, Guid localId)
    {
        Nombre = nombre;
        LocalId = localId;
        Activa = true;
    }

    public string Nombre { get; private set; } = string.Empty;

    public Guid LocalId { get; private set; }

    public bool Activa { get; private set; }
}
