using PosFarmacia.Domain.Enums;

namespace PosFarmacia.Domain.Entities;

public sealed class FormaPago : Entidad
{
    private FormaPago() { }

    public FormaPago(string nombre, TipoFormaPago tipo)
    {
        Nombre = nombre;
        Tipo = tipo;
        Activo = true;
    }

    public string Nombre { get; private set; } = string.Empty;

    public TipoFormaPago Tipo { get; private set; }

    public bool Activo { get; private set; }

    public void Desactivar() => Activo = false;
}
