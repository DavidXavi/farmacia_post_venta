using PosFarmacia.Domain.Enums;
using PosFarmacia.Domain.Exceptions;
using PosFarmacia.Domain.ValueObjects;

namespace PosFarmacia.Domain.Entities;

public sealed class SesionCaja : Entidad
{
    private SesionCaja() { }

    public SesionCaja(Guid cajaId, Guid usuarioId, Dinero montoInicial)
    {
        CajaId = cajaId;
        UsuarioId = usuarioId;
        MontoInicial = montoInicial;
        FechaApertura = DateTime.UtcNow;
        Estado = EstadoCaja.Abierta;
    }

    public Guid CajaId { get; private set; }

    public Guid UsuarioId { get; private set; }

    public DateTime FechaApertura { get; private set; }

    public Dinero MontoInicial { get; private set; } = Dinero.Cero;

    public DateTime? FechaCierre { get; private set; }

    public Dinero? MontoEsperado { get; private set; }

    public Dinero? MontoDeclarado { get; private set; }

    public Dinero? Diferencia { get; private set; }

    public string? ObservacionCierre { get; private set; }

    public EstadoCaja Estado { get; private set; }

    public void Cerrar(Dinero montoEsperado, Dinero montoDeclarado, string? observacion)
    {
        if (Estado == EstadoCaja.Cerrada)
        {
            throw new CajaCerradaException("La sesion de caja ya se encuentra cerrada.");
        }

        MontoEsperado = montoEsperado;
        MontoDeclarado = montoDeclarado;
        Diferencia = new Dinero(Math.Abs(montoDeclarado.Monto - montoEsperado.Monto));
        ObservacionCierre = observacion;
        FechaCierre = DateTime.UtcNow;
        Estado = EstadoCaja.Cerrada;
    }

    public void AsegurarAbierta()
    {
        if (Estado != EstadoCaja.Abierta)
        {
            throw new CajaCerradaException();
        }
    }
}
