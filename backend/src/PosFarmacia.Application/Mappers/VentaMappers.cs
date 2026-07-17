using PosFarmacia.Application.DTOs;
using PosFarmacia.Domain.Entities;

namespace PosFarmacia.Application.Mappers;

public static class VentaMappers
{
    public static VentaResponse ToResponse(this Venta v, IReadOnlyDictionary<Guid, string> nombresProducto)
    {
        var detalles = v.Detalles.Select(d => new DetalleVentaResponse(
            d.Id,
            d.ProductoId,
            nombresProducto.GetValueOrDefault(d.ProductoId, string.Empty),
            d.Cantidad.Valor,
            d.PrecioUnitario.Monto,
            d.PromocionAplicadaId,
            d.DescuentoMonto.Monto,
            d.ImpuestoMonto.Monto,
            d.Subtotal.Monto)).ToList();

        var pagos = v.Pagos.Select(p => new PagoResponse(p.Id, p.FormaPagoId, p.Monto.Monto, p.CodigoAutorizacion)).ToList();

        return new VentaResponse(
            v.Id,
            v.NumeroCorrelativo,
            v.Fecha,
            v.CajaId,
            v.UsuarioId,
            v.ClienteId,
            v.ConvenioSeguroId,
            v.LineaCreditoId,
            v.Estado.ToString(),
            v.Total.Monto,
            v.TotalPagado.Monto,
            detalles,
            pagos,
            v.Comprobante?.Numero.ToString());
    }
}
