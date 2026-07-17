using PosFarmacia.Domain.Enums;

namespace PosFarmacia.Domain.Entities;

public sealed class Rol : Entidad
{
    private Rol() { }

    public Rol(RolNombre nombre, string descripcion)
    {
        Nombre = nombre;
        Descripcion = descripcion;
    }

    public RolNombre Nombre { get; private set; }

    public string Descripcion { get; private set; } = string.Empty;
}
