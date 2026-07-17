using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PosFarmacia.Application.Ports;
using PosFarmacia.Domain.Entities;

namespace PosFarmacia.Infrastructure.Authentication;

public sealed class JwtTokenGenerator(IOptions<JwtOptions> opciones) : IJwtTokenGenerator
{
    private readonly JwtOptions _opciones = opciones.Value;

    public string Generar(Usuario usuario, IReadOnlyCollection<string> roles)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
            new(ClaimTypes.Name, usuario.NombreUsuario),
            new("localId", usuario.LocalId.ToString())
        };
        claims.AddRange(roles.Select(rol => new Claim(ClaimTypes.Role, rol)));

        var credenciales = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_opciones.Key)), SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _opciones.Issuer,
            audience: _opciones.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_opciones.ExpiracionMinutos),
            signingCredentials: credenciales);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
