using PosFarmacia.Domain.Enums;
using PosFarmacia.Domain.Exceptions;
using PosFarmacia.Domain.ValueObjects;

namespace PosFarmacia.Domain.Entities;

public sealed class Receta : Entidad
{
    private Receta() { }

    public Receta(
        NumeroReceta numero,
        TipoReceta tipo,
        DateOnly fechaEmision,
        DateOnly? fechaVencimiento,
        Guid productoId,
        string datosPaciente,
        string datosProfesional,
        string dosisYCantidadAutorizada,
        Guid? clienteId = null,
        string? archivoRespaldoUrl = null)
    {
        if (tipo != TipoReceta.Normal && fechaVencimiento is null)
        {
            throw new ValorInvalidoException("Las recetas especiales requieren fecha de vencimiento.");
        }

        Numero = numero;
        Tipo = tipo;
        FechaEmision = fechaEmision;
        FechaVencimiento = fechaVencimiento;
        ProductoId = productoId;
        ClienteId = clienteId;
        DatosPaciente = datosPaciente;
        DatosProfesional = datosProfesional;
        DosisYCantidadAutorizada = dosisYCantidadAutorizada;
        ArchivoRespaldoUrl = archivoRespaldoUrl;
        Estado = EstadoReceta.Pendiente;
    }

    public NumeroReceta Numero { get; private set; } = null!;

    public TipoReceta Tipo { get; private set; }

    public DateOnly FechaEmision { get; private set; }

    public DateOnly? FechaVencimiento { get; private set; }

    public Guid ProductoId { get; private set; }

    public Guid? ClienteId { get; private set; }

    public string DatosPaciente { get; private set; } = string.Empty;

    public string DatosProfesional { get; private set; } = string.Empty;

    public string DosisYCantidadAutorizada { get; private set; } = string.Empty;

    public string? ArchivoRespaldoUrl { get; private set; }

    public EstadoReceta Estado { get; private set; }

    public bool RetenidaEnBotica { get; private set; }

    public bool EstaVencida(DateOnly hoy) => FechaVencimiento is not null && FechaVencimiento < hoy;

    public void Aprobar() => Estado = EstadoReceta.Aprobada;

    public void Rechazar() => Estado = EstadoReceta.Rechazada;

    public void MarcarUtilizadaYRetenida()
    {
        if (Tipo != TipoReceta.EspecialRetenida)
        {
            return;
        }

        if (Estado == EstadoReceta.Utilizada)
        {
            throw new RecetaYaUtilizadaException();
        }

        Estado = EstadoReceta.Utilizada;
        RetenidaEnBotica = true;
    }

    public bool PuedeUsarseNuevamente() => Tipo != TipoReceta.EspecialRetenida || Estado != EstadoReceta.Utilizada;
}
