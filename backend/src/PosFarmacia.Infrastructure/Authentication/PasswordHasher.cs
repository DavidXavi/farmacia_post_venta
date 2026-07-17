using System.Security.Cryptography;
using PosFarmacia.Application.Ports;

namespace PosFarmacia.Infrastructure.Authentication;

/// PBKDF2 con las utilidades de System.Security.Cryptography: sin dependencias externas.
public sealed class PasswordHasher : IPasswordHasher
{
    private const int TamanoSal = 16;
    private const int TamanoHash = 32;
    private const int Iteraciones = 100_000;
    private static readonly HashAlgorithmName Algoritmo = HashAlgorithmName.SHA256;

    public string Hash(string password)
    {
        var sal = RandomNumberGenerator.GetBytes(TamanoSal);
        var hash = Rfc2898DeriveBytes.Pbkdf2(password, sal, Iteraciones, Algoritmo, TamanoHash);
        return $"{Iteraciones}.{Convert.ToBase64String(sal)}.{Convert.ToBase64String(hash)}";
    }

    public bool Verify(string password, string hash)
    {
        var partes = hash.Split('.');
        if (partes.Length != 3 || !int.TryParse(partes[0], out var iteraciones))
        {
            return false;
        }

        var sal = Convert.FromBase64String(partes[1]);
        var hashEsperado = Convert.FromBase64String(partes[2]);
        var hashCalculado = Rfc2898DeriveBytes.Pbkdf2(password, sal, iteraciones, Algoritmo, hashEsperado.Length);

        return CryptographicOperations.FixedTimeEquals(hashCalculado, hashEsperado);
    }
}
