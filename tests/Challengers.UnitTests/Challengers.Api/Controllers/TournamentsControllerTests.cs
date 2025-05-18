using Challengers.Application.DTOs;
using Challengers.Domain.Enums;
using FluentAssertions;
using System.Net.Http.Json;

namespace Challengers.UnitTests.Challengers.Api.Controllers;

public class TournamentsControllerTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task PostTournaments_ShouldReturnCreatedTournament()
    {
        // Arrange
        var request = new
        {
            name = "Torneo Test",
            gender = 2,
            savePlayers = false,
            players = new[]
            {
                new { name = "Ana", surname = "Uno", skill = 80, reactionTime = 85, gender = 2 },
                new { name = "Laura", surname = "Dos", skill = 85, reactionTime = 90, gender = 2 }
            }
        };

        // Act
        var response = await _client.PostAsJsonAsync("api/tournaments", request);

        // Assert
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        json.Should().Contain("Torneo Test");
    }

    [Fact]
    public async Task PutSimulate_ShouldReturnSimulatedResult()
    {
        // Arrange
        var request = new
        {
            name = "Torneo Simulable",
            gender = 1,
            savePlayers = false,
            players = new[]
            {
            new { name = "Juan", surname = "Uno", skill = 80, strength = 85, speed = 75, gender = 1 },
            new { name = "Pedro", surname = "Dos", skill = 78, strength = 82, speed = 74, gender = 0 }
        }
        };

        var createResponse = await _client.PostAsJsonAsync("/api/tournaments", request);
        createResponse.EnsureSuccessStatusCode();
        var created = await createResponse.Content.ReadFromJsonAsync<CreateTournamentResponseDto>();

        // Act
        var simulateResponse = await _client.PutAsync($"/api/tournaments/{created!.TournamentId}/simulate", null);

        // Assert
        simulateResponse.EnsureSuccessStatusCode();
        var simulationResult = await simulateResponse.Content.ReadFromJsonAsync<SimulateTournamentResponseDto>();

        simulationResult!.TournamentId.Should().Be(created.TournamentId);
        simulationResult.Winner.Should().BeOneOf("Juan Uno", "Pedro Dos");
    }

    [Fact]
    public async Task GetById_ShouldReturnTournamentResult()
    {
        // Arrange
        var request = new
        {
            name = "Torneo Final",
            gender = 1,
            savePlayers = false,
            players = new[]
            {
            new { name = "Juan", surname = "Uno", skill = 80, strength = 85, speed = 75, gender = 1 },
            new { name = "Pedro", surname = "Dos", skill = 78, strength = 82, speed = 74, gender = 1 }
        }
        };

        var createResponse = await _client.PostAsJsonAsync("/api/tournaments", request);
        createResponse.EnsureSuccessStatusCode();
        var created = await createResponse.Content.ReadFromJsonAsync<CreateTournamentResponseDto>();

        var simulateResponse = await _client.PutAsync($"/api/tournaments/{created!.TournamentId}/simulate", null);
        simulateResponse.EnsureSuccessStatusCode();

        // Act
        var getResponse = await _client.GetAsync($"/api/tournaments/{created.TournamentId}");

        // Assert
        getResponse.EnsureSuccessStatusCode();
        var tournamentResult = await getResponse.Content.ReadFromJsonAsync<TournamentResultDto>();

        tournamentResult.Should().NotBeNull();
        tournamentResult!.Id.Should().Be(created.TournamentId);
        tournamentResult.Name.Should().Be("Torneo Final");
        tournamentResult.Winner.Should().BeOneOf("Juan Uno", "Pedro Dos");
        tournamentResult.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task GetAll_ShouldFilterByGenderAndDate()
    {
        // Arrange
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        var femaleRequest = new
        {
            name = "Female Tournament",
            gender = 2,
            savePlayers = false,
            players = new[]
            {
            new { name = "Ana", surname = "Uno", skill = 85, reactionTime = 90, gender = 2 },
            new { name = "Laura", surname = "Dos", skill = 82, reactionTime = 88, gender = 2 }
        }
        };

        var maleRequest = new
        {
            name = "Male Tournament",
            gender = 1,
            savePlayers = false,
            players = new[]
            {
            new { name = "Juan", surname = "Tres", skill = 80, strength = 85, speed = 75, gender = 1 },
            new { name = "Pedro", surname = "Cuatro", skill = 78, strength = 82, speed = 74, gender = 1 }
        }
        };

        var fResp = await _client.PostAsJsonAsync("/api/tournaments", femaleRequest);
        var fCreated = await fResp.Content.ReadFromJsonAsync<TournamentResultDto>();
        await _client.PutAsync($"/api/tournaments/{fCreated!.Id}/simulate", null);

        var mResp = await _client.PostAsJsonAsync("/api/tournaments", maleRequest);
        var mCreated = await mResp.Content.ReadFromJsonAsync<TournamentResultDto>();
        await _client.PutAsync($"/api/tournaments/{mCreated!.Id}/simulate", null);

        // Act
        var filterUrl = $"/api/tournaments?gender=2&date={today:yyyy-MM-dd}";
        var response = await _client.GetAsync(filterUrl);

        // Assert
        response.EnsureSuccessStatusCode();
        var results = await response.Content.ReadFromJsonAsync<PagedResultDto<TournamentResultDto>>();

        results!.Items.Should().ContainSingle();
        results.Items[0].Name.Should().Be("Female Tournament");
        results.Items[0].Gender.Should().Be(Gender.Female);
        results.Items[0].CreatedAt.Date.Should().Be(DateTime.UtcNow.Date);
    }
}
