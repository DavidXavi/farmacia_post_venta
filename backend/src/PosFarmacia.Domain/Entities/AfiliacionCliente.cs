using PosFarmacia.Domain.Enums;
using PosFarmacia.Domain.ValueObjects;

namespace PosFarmacia.Domain.Entities;

public sealed class AfiliacionCliente : Entidad
{
    private AfiliacionCliente() { }

    public AfiliacionCliente(Guid clienteId, Guid convenioId, DateOnly? vigenciaInicio, DateOnly? vigenciaFin)
    {
        ClienteId = clienteId;
        ConvenioId = convenioId;
        Vigencia = new PeriodoVigencia(vigenciaInicio, vigenciaFin);
        Estado = EstadoAfiliacion.Activa;
    }

    public Guid ClienteId { get; private set; }

    public Guid ConvenioId { get; private set; }

    public PeriodoVigencia Vigencia { get; private set; } = null!;

    public EstadoAfiliacion Estado { get; private set; }

    public bool EstaActivaYVigente(DateOnly hoy) => Estado == EstadoAfiliacion.Activa && Vigencia.EstaVigente(hoy);

    public void Suspender() => Estado = EstadoAfiliacion.Suspendida;

    public void Reactivar() => Estado = EstadoAfiliacion.Activa;
}


