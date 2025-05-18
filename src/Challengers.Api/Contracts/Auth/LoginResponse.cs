namespace Challengers.Api.Contracts.Auth;

public class LoginResponse
{
    public string Token { get; set; } = default!;
    public DateTime ExpiresAt { get; set; }
}

