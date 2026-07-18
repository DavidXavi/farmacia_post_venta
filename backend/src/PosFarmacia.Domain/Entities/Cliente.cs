using PosFarmacia.Domain.Enums;
using PosFarmacia.Domain.ValueObjects;

namespace PosFarmacia.Domain.Entities;

public sealed class Cliente : Entidad
{
    private Cliente() { }

    public Cliente(Dni dni, string nombres, string apellidos, DateOnly? fechaNacimiento = null,
        string? telefono = null, string? correo = null, string? direccion = null)
    {
        Dni = dni;
        Nombres = nombres;
        Apellidos = apellidos;
        FechaNacimiento = fechaNacimiento;
        Telefono = telefono;
        Correo = correo;
        Direccion = direccion;
        Estado = EstadoCuenta.Activo;
    }

    public Dni Dni { get; private set; } = null!;

    public string Nombres { get; private set; } = string.Empty;

    public string Apellidos { get; private set; } = string.Empty;

    public DateOnly? FechaNacimiento { get; private set; }

    public string? Telefono { get; private set; }

    public string? Correo { get; private set; }

    public string? Direccion { get; private set; }

    public EstadoCuenta Estado { get; private set; }

    public string NombreCompleto => $"{Nombres} {Apellidos}";

    public void ActualizarDatos(string? telefono, string? correo, string? direccion)
    {
        Telefono = telefono;
        Correo = correo;
        Direccion = direccion;
    }
}


