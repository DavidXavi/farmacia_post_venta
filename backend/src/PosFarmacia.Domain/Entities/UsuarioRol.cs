namespace PosFarmacia.Domain.Entities;

public sealed class UsuarioRol
{
    private UsuarioRol() { }

    internal UsuarioRol(Guid usuarioId, Guid rolId)
    {
        UsuarioId = usuarioId;
        RolId = rolId;
    }

    public Guid UsuarioId { get; private set; }

    public Guid RolId { get; private set; }
}


