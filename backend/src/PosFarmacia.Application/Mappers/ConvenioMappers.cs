using PosFarmacia.Application.DTOs;
using PosFarmacia.Domain.Entities;

namespace PosFarmacia.Application.Mappers;

public static class ConvenioMappers
{
    public static ConvenioResponse ToResponse(this ConvenioSeguro c) => new(c.Id, c.Nombre, c.Activo);

    public static AfiliacionResponse ToResponse(this AfiliacionCliente a) => new(
        a.Id, a.ClienteId, a.ConvenioId, a.Estado.ToString(), a.Vigencia.Inicio, a.Vigencia.Fin);
}
