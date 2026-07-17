namespace PosFarmacia.Domain.Enums;

public enum RolNombre
{
    Administrador,
    Cajero,
    QuimicoFarmaceutico,
    EncargadoInventario,
    OperadorCentral
}

public enum EstadoCuenta
{
    Activo,
    Inactivo,
    Suspendido
}

public enum EstadoCaja
{
    Cerrada,
    Abierta
}

public enum TipoProducto
{
    Medicamento,
    Otc,
    Otro
}

public enum EstadoProducto
{
    Activo,
    Suspendido,
    DadoDeBaja
}

public enum TipoReceta
{
    Normal,
    Especial,
    EspecialRetenida
}

public enum EstadoReceta
{
    Pendiente,
    Aprobada,
    Rechazada,
    Utilizada
}

public enum EstadoLote
{
    Disponible,
    Bloqueado,
    Retirado,
    Agotado,
    Vencido
}

public enum EstadoVenta
{
    EnProceso,
    Confirmada,
    Anulada
}

public enum TipoMovimientoStock
{
    Ingreso,
    Salida,
    AjustePositivo,
    AjusteNegativo,
    ReversionAnulacion
}

public enum TipoComprobante
{
    Boleta,
    Factura,
    Ticket
}

public enum EstadoAfiliacion
{
    Activa,
    Inactiva,
    Suspendida
}

public enum EstadoLineaCredito
{
    Activa,
    Inactiva,
    Bloqueada
}

public enum TipoFormaPago
{
    Efectivo,
    TarjetaDebito,
    TarjetaCredito,
    Transferencia,
    BilleteraDigital,
    CopagoSeguro,
    CreditoFarmacia,
    Otro
}

public enum TipoMovimientoCredito
{
    Consumo,
    Reversion
}

[Flags]
public enum PermisoEspecial
{
    Ninguno = 0,
    AnularVentas = 1,
    ValidarRecetas = 2,
    AjustarStock = 4,
    VerAuditoria = 8,
    EmitirNotaCredito = 16
}

public enum TipoBeneficioPromocion
{
    DescuentoPorcentaje,
    DescuentoMonto,
    LlevaNPagaM
}
