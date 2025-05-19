using Challengers.Api.Contracts.Auth;
using Challengers.Application.DTOs;
using Challengers.Domain.Enums;
using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Challengers.AuthTests;

public class AuthControllerTests(CustomAuthWebApplicationFactoryForAuth factory) : IClassFixture<CustomAuthWebApplicationFactoryForAuth>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task Login_ShouldReturnToken_WhenCredentialsAreCorrect()
    {
        // Arrange
        var login = new { Username = "admin", Password = "secret" };

        // Act
        var response = await _client.PostAsJsonAsync("/auth", login);
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
        result!.Token.Should().NotBeNullOrWhiteSpace();
        result.ExpiresAt.Should().BeAfter(DateTime.UtcNow);
    }

    [Fact]
    public async Task Login_ShouldReturnUnauthorized_WhenCredentialsAreInvalid()
    {
        // Arrange
        var login = new { Username = "wrong", Password = "wrong" };

        // Act
        var response = await _client.PostAsJsonAsync("/auth", login);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task PostPlayer_ShouldRequireAuthorization()
    {
        // Arrange
        var dto = new CreatePlayerRequestDto
        {
            FirstName = "Infiltrado",
            LastName = "NoAuth",
            Gender = Gender.Male,
            Skill = 70,
            Strength = 80,
            Speed = 80
        };

        // Act – sin token
        var response = await _client.PostAsJsonAsync("/api/players", dto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetPlayers_ShouldReturnOk_WhenAuthorized()
    {
        // Arrange
        var login = new { Username = "admin", Password = "secret" };
        var loginResponse = await _client.PostAsJsonAsync("/auth", login);
        var result = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>();

        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", result!.Token);

        // Act
        var response = await _client.GetAsync("/api/players");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

}
