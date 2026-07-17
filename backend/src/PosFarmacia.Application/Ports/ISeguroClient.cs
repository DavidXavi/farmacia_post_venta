using PosFarmacia.Domain.Entities;

namespace PosFarmacia.Application.Ports;

/// Puerto hacia el servicio de convenios de seguro (central). RN27: si la consulta no puede confirmar
/// afiliacion activa y vigente, retorna null; la venta no debe asumir cobertura de forma automatica.
public interface ISeguroClient
{
    Task<ConvenioSeguro?> ConsultarConvenioVigenteAsync(Guid clienteId, Guid convenioId, DateOnly hoy, CancellationToken ct = default);
}
