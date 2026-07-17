using PosFarmacia.Application.DTOs;
using PosFarmacia.Domain.Entities;

namespace PosFarmacia.Application.Mappers;

public static class ClienteMappers
{
    public static ClienteResponse ToResponse(this Cliente c) => new(
        c.Id,
        c.Dni.Valor,
        c.Nombres,
        c.Apellidos,
        c.FechaNacimiento,
        c.Telefono,
        c.Correo,
        c.Direccion,
        c.Estado.ToString());
}
