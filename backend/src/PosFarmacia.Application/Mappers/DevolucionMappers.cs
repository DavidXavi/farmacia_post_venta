using PosFarmacia.Application.DTOs;
using PosFarmacia.Domain.Entities;

namespace PosFarmacia.Application.Mappers;

public static class DevolucionMappers
{
    public static DetalleDevolucionResponse ToResponse(this DetalleDevolucion d) =>
        new(d.Id, d.DetalleVentaId, d.ProductoId, d.Cantidad.Valor, d.MontoDevuelto.Monto);

    public static DevolucionResponse ToResponse(this Devolucion d) =>
        new(d.Id, d.VentaId, d.UsuarioId, d.Motivo, d.Fecha, d.Total.Monto, d.Detalles.Select(x => x.ToResponse()).ToList());
}
