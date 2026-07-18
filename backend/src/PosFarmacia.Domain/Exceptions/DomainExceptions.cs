namespace PosFarmacia.Domain.Exceptions;

public abstract class DomainException(string message) : Exception(message);

public sealed class ValorInvalidoException(string mensaje) : DomainException(mensaje);

public sealed class EntidadNoEncontradaException(string mensaje) : DomainException(mensaje);

public sealed class CajaCerradaException(string mensaje = "No se puede registrar una venta sin una sesion de caja abierta.")
    : DomainException(mensaje);

public sealed class StockInsuficienteException(string mensaje = "No hay stock vendible suficiente para la cantidad solicitada.")
    : DomainException(mensaje);

public sealed class PagoInsuficienteException(string mensaje = "El monto pagado no cubre el total que corresponde al cliente.")
    : DomainException(mensaje);

public sealed class VentaYaConfirmadaException(string mensaje = "La venta ya fue confirmada y no puede modificarse.")
    : DomainException(mensaje);

public sealed class PromocionInvalidaException(string mensaje) : DomainException(mensaje);

public sealed class RecetaInvalidaException(string mensaje) : DomainException(mensaje);

public sealed class RecetaYaUtilizadaException(string mensaje = "La receta especial retenida ya fue utilizada en otra venta.")
    : DomainException(mensaje);

public sealed class ConvenioNoDisponibleException(string mensaje) : DomainException(mensaje);

public sealed class LineaCreditoInvalidaException(string mensaje) : DomainException(mensaje);

public sealed class AnulacionNoPermitidaException(string mensaje) : DomainException(mensaje);

public sealed class DevolucionInvalidaException(string mensaje) : DomainException(mensaje);
