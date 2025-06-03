namespace GameStore.Api.Authorization;

public class Auth0Settings
{
    public string Authority { get; set; } = string.Empty;
    public List<string> ValidAudiences { get; set; } = new();
}