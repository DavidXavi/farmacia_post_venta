namespace PosFarmacia.Infrastructure.Authentication;

public sealed class JwtOptions
{
    public const string SeccionConfiguracion = "Jwt";

    public string Key { get; set; } = string.Empty;

    public string Issuer { get; set; } = "PosFarmacia";

    public string Audience { get; set; } = "PosFarmacia";

    public int ExpiracionMinutos { get; set; } = 480;
}
