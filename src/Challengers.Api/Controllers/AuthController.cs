using Challengers.Api.Contracts.Auth;
using Challengers.Infrastructure.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Challengers.Api.Controllers;

[ApiController]
[Route("auth")]
public class AuthController(JwtTokenGenerator tokenGenerator, IConfiguration configuration) : ControllerBase
{
    private readonly JwtTokenGenerator _tokenGenerator = tokenGenerator;
    private readonly IConfiguration _configuration = configuration;

    [AllowAnonymous]
    [HttpPost]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var adminUsername = _configuration["AdminCredentials:Username"];
        var adminPassword = _configuration["AdminCredentials:Password"];

        if (request.Username != adminUsername || request.Password != adminPassword)
            return Unauthorized("Invalid credentials");

        var token = _tokenGenerator.GenerateToken(request.Username);
        var expiresAt = DateTime.UtcNow.AddMinutes(
            int.Parse(_configuration["JwtSettings:ExpirationMinutes"] ?? "60")
        );

        return Ok(new LoginResponse
        {
            Token = token,
            ExpiresAt = expiresAt
        });
    }
}
