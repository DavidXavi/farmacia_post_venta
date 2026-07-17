namespace PosFarmacia.Application.DTOs;

public sealed record IniciarVentaRequest(Guid CajaId, Guid SesionCajaId, Guid UsuarioId, string? ClienteDni);

public sealed record AgregarProductoRequest(Guid ProductoId, int Cantidad, Guid? RecetaId);

public sealed record SeleccionarPromocionRequest(Guid PromocionId);

public sealed record IdentificarClienteRequest(string Dni);

public sealed record AplicarConvenioRequest(Guid ConvenioId);

public sealed record RegistrarPagoRequest(Guid FormaPagoId, decimal Monto, string? CodigoAutorizacion);

public sealed record ConfirmarVentaRequest(string TipoComprobante, string SerieComprobante);

public sealed record AnularVentaRequest(Guid UsuarioId, string Motivo);

public sealed record DetalleVentaResponse(
    Guid Id,
    Guid ProductoId,
    string NombreProducto,
    int Cantidad,
    decimal PrecioUnitario,
    Guid? PromocionAplicadaId,
    decimal DescuentoMonto,
    decimal ImpuestoMonto,
    decimal Subtotal);

public sealed record PagoResponse(Guid Id, Guid FormaPagoId, decimal Monto, string? CodigoAutorizacion);

public sealed record VentaResponse(
    Guid Id,
    long? NumeroCorrelativo,
    DateTime Fecha,
    Guid CajaId,
    Guid UsuarioId,
    Guid? ClienteId,
    Guid? ConvenioSeguroId,
    Guid? LineaCreditoId,
    string Estado,
    decimal Total,
    decimal TotalPagado,
    IReadOnlyCollection<DetalleVentaResponse> Detalles,
    IReadOnlyCollection<PagoResponse> Pagos,
    string? NumeroComprobante);

public sealed record PromocionAplicableResponse(Guid Id, string Nombre, string TipoBeneficio, decimal ValorBeneficio, bool RequiereCliente);
