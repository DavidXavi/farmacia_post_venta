namespace PosFarmacia.Domain.Entities;

public sealed class Local : Entidad
{
    private Local() { }

    public Local(string nombre, string direccion)
    {
        Nombre = nombre;
        Direccion = direccion;
        Activo = true;
    }

    public string Nombre { get; private set; } = string.Empty;

    public string Direccion { get; private set; } = string.Empty;

    public bool Activo { get; private set; }

    public void Desactivar() => Activo = false;
}
