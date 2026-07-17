using PosFarmacia.Domain.Enums;
using PosFarmacia.Domain.ValueObjects;

namespace PosFarmacia.Domain.Entities;

public sealed class Promocion : Entidad
{
    private readonly List<PromocionProducto> _productos = [];

    private Promocion() { }

    public Promocion(
        string nombre,
        string descripcion,
        TipoBeneficioPromocion tipoBeneficio,
        decimal valorBeneficio,
        bool requiereCliente,
        Cantidad cantidadMinima,
        DateOnly? fechaInicio,
        DateOnly? fechaFin)
    {
        Nombre = nombre;
        Descripcion = descripcion;
        TipoBeneficio = tipoBeneficio;
        ValorBeneficio = valorBeneficio;
        RequiereCliente = requiereCliente;
        CantidadMinima = cantidadMinima;
        Vigencia = new PeriodoVigencia(fechaInicio, fechaFin);
        Activa = true;
    }

    public string Nombre { get; private set; } = string.Empty;

    public string Descripcion { get; private set; } = string.Empty;

    public TipoBeneficioPromocion TipoBeneficio { get; private set; }

    public decimal ValorBeneficio { get; private set; }

    public bool RequiereCliente { get; private set; }

    public Cantidad CantidadMinima { get; private set; } = new(1);

    public PeriodoVigencia Vigencia { get; private set; } = null!;

    public bool Activa { get; private set; }

    public IReadOnlyCollection<PromocionProducto> Productos => _productos;

    public void AgregarProductoParticipante(Guid productoId)
    {
        if (_productos.Any(p => p.ProductoId == productoId))
        {
            return;
        }

        _productos.Add(new PromocionProducto(Id, productoId));
    }

    public bool AplicaAProducto(Guid productoId) => _productos.Any(p => p.ProductoId == productoId);

    public bool EstaVigente(DateOnly hoy) => Activa && Vigencia.EstaVigente(hoy);

    public Dinero CalcularDescuento(Dinero precioUnitario, Cantidad cantidad)
    {
        return TipoBeneficio switch
        {
            TipoBeneficioPromocion.DescuentoPorcentaje =>
                new Dinero(new Porcentaje(ValorBeneficio).AplicarSobre(precioUnitario.Monto * cantidad.Valor)),
            TipoBeneficioPromocion.DescuentoMonto =>
                new Dinero(Math.Min(ValorBeneficio, precioUnitario.Monto * cantidad.Valor)),
            TipoBeneficioPromocion.LlevaNPagaM =>
                CalcularDescuentoLlevaNPagaM(precioUnitario, cantidad),
            _ => Dinero.Cero
        };
    }

    private Dinero CalcularDescuentoLlevaNPagaM(Dinero precioUnitario, Cantidad cantidad)
    {
        var n = (int)ValorBeneficio;
        if (n <= 0)
        {
            return Dinero.Cero;
        }

        var unidadesGratis = cantidad.Valor / n;
        return new Dinero(precioUnitario.Monto * unidadesGratis);
    }

    public void Desactivar() => Activa = false;
}
