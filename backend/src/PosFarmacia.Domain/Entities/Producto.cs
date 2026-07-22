using PosFarmacia.Domain.Enums;
using PosFarmacia.Domain.ValueObjects;

namespace PosFarmacia.Domain.Entities;

public sealed class Producto : Entidad
{
    private Producto() { }

    public Producto(
        CodigoProducto codigoInterno,
        string nombreComercial,
        string descripcion,
        TipoProducto tipoProducto,
        Guid categoriaId,
        Guid laboratorioId,
        Guid presentacionId,
        Dinero precioVenta,
        bool esControlado,
        bool requiereReceta,
        TipoReceta? tipoRecetaRequerida,
        CodigoBarras? codigoBarras = null)
    {
        if (esControlado && !requiereReceta)
        {
            throw new Exceptions.ValorInvalidoException("Un producto controlado siempre debe requerir receta.");
        }

        CodigoInterno = codigoInterno;
        CodigoBarras = codigoBarras;
        NombreComercial = nombreComercial;
        Descripcion = descripcion;
        TipoProducto = tipoProducto;
        CategoriaId = categoriaId;
        LaboratorioId = laboratorioId;
        PresentacionId = presentacionId;
        PrecioVenta = precioVenta;
        EsControlado = esControlado;
        RequiereReceta = requiereReceta;
        TipoRecetaRequerida = tipoRecetaRequerida;
        Estado = EstadoProducto.Activo;
    }

    public CodigoProducto CodigoInterno { get; private set; } = null!;

    public CodigoBarras? CodigoBarras { get; private set; }

    public string NombreComercial { get; private set; } = string.Empty;

    public string Descripcion { get; private set; } = string.Empty;

    public TipoProducto TipoProducto { get; private set; }

    public Guid CategoriaId { get; private set; }

    public Guid LaboratorioId { get; private set; }

    public Guid PresentacionId { get; private set; }

    public Dinero PrecioVenta { get; private set; } = Dinero.Cero;

    public bool EsControlado { get; private set; }

    public bool RequiereReceta { get; private set; }

    public TipoReceta? TipoRecetaRequerida { get; private set; }

    public EstadoProducto Estado { get; private set; }

    public void ActualizarPrecio(Dinero nuevoPrecio) => PrecioVenta = nuevoPrecio;

    public void ActualizarDatos(string nombreComercial, string descripcion, Dinero precioVenta)
    {
        NombreComercial = nombreComercial;
        Descripcion = descripcion;
        PrecioVenta = precioVenta;
    }

    public void DarDeBaja() => Estado = EstadoProducto.DadoDeBaja;

    public void Suspender() => Estado = EstadoProducto.Suspendido;

    public void Reactivar() => Estado = EstadoProducto.Activo;

    public bool EstaVendible => Estado == EstadoProducto.Activo;
}


