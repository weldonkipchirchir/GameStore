namespace GameStore.Api.Authorization;

public class JwtSettings
{
    public required string Key { get; set; }
    public required string Issuer { get; set; }
}