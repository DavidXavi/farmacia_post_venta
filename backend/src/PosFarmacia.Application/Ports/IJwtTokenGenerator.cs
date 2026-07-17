using PosFarmacia.Domain.Entities;

namespace PosFarmacia.Application.Ports;

public interface IJwtTokenGenerator
{
    string Generar(Usuario usuario, IReadOnlyCollection<string> roles);
}
