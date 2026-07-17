using PosFarmacia.Domain.Entities;
using PosFarmacia.Domain.ValueObjects;

namespace PosFarmacia.Domain.Services;

public sealed class EvaluadorPromociones
{
    public IReadOnlyList<Promocion> ObtenerAplicables(
        IEnumerable<Promocion> promocionesDelProducto,
        Cantidad cantidad,
        bool clienteIdentificado,
        DateOnly hoy)
    {
        return promocionesDelProducto
            .Where(p => p.EstaVigente(hoy))
            .Where(p => cantidad >= p.CantidadMinima)
            .Where(p => !p.RequiereCliente || clienteIdentificado)
            .ToList();
    }
}
