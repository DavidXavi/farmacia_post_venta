using PosFarmacia.Domain.Enums;

namespace PosFarmacia.Domain.Entities;

public sealed class Usuario : Entidad
{
    private readonly List<UsuarioRol> _roles = [];

    private Usuario() { }

    public Usuario(string nombreUsuario, string passwordHash, Guid localId, PermisoEspecial permisos = PermisoEspecial.Ninguno)
    {
        NombreUsuario = nombreUsuario;
        PasswordHash = passwordHash;
        LocalId = localId;
        Permisos = permisos;
        Estado = EstadoCuenta.Activo;
    }

    public string NombreUsuario { get; private set; } = string.Empty;

    public string PasswordHash { get; private set; } = string.Empty;

    public EstadoCuenta Estado { get; private set; }

    public Guid LocalId { get; private set; }

    public PermisoEspecial Permisos { get; private set; }

    public IReadOnlyCollection<UsuarioRol> Roles => _roles;

    public void AsignarRol(Guid rolId)
    {
        if (_roles.Any(r => r.RolId == rolId))
        {
            return;
        }

        _roles.Add(new UsuarioRol(Id, rolId));
    }

    public bool TienePermiso(PermisoEspecial permiso) => Permisos.HasFlag(permiso);

    public void Suspender() => Estado = EstadoCuenta.Suspendido;

    public void Activar() => Estado = EstadoCuenta.Activo;

    public bool EstaActivo => Estado == EstadoCuenta.Activo;
}


