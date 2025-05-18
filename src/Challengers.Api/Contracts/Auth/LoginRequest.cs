namespace Challengers.Api.Contracts.Auth;

public class LoginRequest
{
    public string Username { get; set; } = default!;
    public string Password { get; set; } = default!;
}