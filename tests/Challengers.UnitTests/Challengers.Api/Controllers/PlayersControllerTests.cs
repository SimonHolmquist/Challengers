using Challengers.Application.DTOs;
using FluentAssertions;
using System.Net.Http.Json;
using Challengers.Domain.Enums;
using System.Text.Json;
using System.Net;
using Challengers.UnitTests.Helpers;
using System.Globalization;

namespace Challengers.UnitTests.Challengers.Api.Controllers;
public class PlayersControllerTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();
    private static readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    [Fact]
    public async Task GetAll_ShouldFilterByGenderAndName()
    {
        // Arrange
        var uniqueName1 = $"Ana_{Guid.NewGuid().ToString("N")[..8]}";
        var uniqueName2 = $"Carlos_{Guid.NewGuid().ToString("N")[..8]}";
        var female1 = new CreatePlayerRequestDto
        {
            Name = uniqueName1,
            Surname = "Perez",
            Gender = Gender.Female,
            Skill = 80,
            ReactionTime = 90
        };

        var male1 = new CreatePlayerRequestDto
        {
            Name = uniqueName2,
            Surname = "Lopez",
            Gender = Gender.Male,
            Skill = 85,
            Strength = 80,
            Speed = 85
        };

        await _client.PostAsJsonAsync("/api/players", female1);
        await _client.PostAsJsonAsync("/api/players", male1);

        // Act
        var response = await _client.GetAsync($"/api/players?gender=2&name={uniqueName1}");

        // Assert
        response.EnsureSuccessStatusCode();
        var raw = await response.Content.ReadAsStringAsync();

        var paged = JsonSerializer.Deserialize<PagedResultDto<PlayerDto>>(raw, _jsonSerializerOptions);

        paged!.Items.Should().ContainSingle();
        paged.Items[0].Name!.Should().Be(uniqueName1);
        paged.Items[0].Gender.Should().Be(Gender.Female);
    }

    [Fact]
    public async Task Post_ShouldCreatePlayerSuccessfully()
    {
        // Arrange
        var request = new CreatePlayerRequestDto
        {
            Name = "Lucía",
            Surname = "Martínez",
            Gender = Gender.Female,
            Skill = 78,
            ReactionTime = 85
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/players", request);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var raw = await response.Content.ReadAsStringAsync();

        var listResponse = await _client.GetAsync("/api/players?gender=2&name=Lucía&page=1&pageSize=10");
        listResponse.EnsureSuccessStatusCode();
        var listRaw = await listResponse.Content.ReadAsStringAsync();

        var paged = JsonSerializer.Deserialize<PagedResultDto<PlayerDto>>(listRaw, _jsonSerializerOptions);

        paged!.Items.Should().ContainSingle(p => p.Name == "Lucía" && p.Surname == "Martínez");
    }

    [Fact]
    public async Task Post_ShouldFail_WhenDataIsInvalid()
    {
        // Arrange
        var request = new CreatePlayerRequestDto
        {
            Name = "Carlos",
            Surname = "Error",
            Gender = Gender.Male,
            Skill = 105,
            Strength = null,
            Speed = null

        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/players", request);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);

        var raw = await response.Content.ReadAsStringAsync();

        raw.Should().ContainAll("Skill", "Strength", "Speed");
    }

    [Fact]
    public async Task Put_ShouldUpdatePlayerSuccessfully()
    {
        // Arrange
        var uniqueName = $"Lucía_{Guid.NewGuid().ToString("N")[..8]}";
        var create = new CreatePlayerRequestDto
        {
            Name = uniqueName,
            Surname = "Martínez",
            Gender = Gender.Female,
            Skill = 70,
            ReactionTime = 80
        };

        var createResponse = await _client.PostAsJsonAsync("/api/players", create);
        createResponse.EnsureSuccessStatusCode();

        var listResponse = await _client.GetAsync($"/api/players?name={uniqueName}&page=1&pageSize=10");
        var listRaw = await listResponse.Content.ReadAsStringAsync();
        var paged = JsonSerializer.Deserialize<PagedResultDto<PlayerDto>>(listRaw, _jsonSerializerOptions);
        var playerId = paged!.Items.Single().Id;

        // Act
        var update = new UpdatePlayerRequestDto
        {
            Name = "Luciana",
            Skill = 90
        };

        var putResponse = await _client.PutAsJsonAsync($"/api/players/{playerId}", update);

        // Assert
        putResponse.EnsureSuccessStatusCode();

        var updatedResponse = await _client.GetAsync($"/api/players/{playerId}");
        var updatedRaw = await updatedResponse.Content.ReadAsStringAsync();

        var updated = JsonSerializer.Deserialize<PlayerDto>(updatedRaw, _jsonSerializerOptions);

        updated!.Name.Should().Be("Luciana");
        updated.Skill.Should().Be(90);
    }

    [Fact]
    public async Task Put_ShouldUpdateOnlyName_WhenOtherFieldsMissing()
    {
        // Arrange
        var uniqueName = $"Lucía_{Guid.NewGuid().ToString("N")[..8]}";
        var create = new CreatePlayerRequestDto
        {
            Name = uniqueName,
            Surname = "Martínez",
            Gender = Gender.Female,
            Skill = 75,
            ReactionTime = 88
        };

        var playerId = await TestHelper.CreateAndGetPlayerIdAsync(create, _client);

        var update = new UpdatePlayerRequestDto
        {
            Name = "Luciana"
        };

        // Act & Assert
        var putResponse = await _client.PutAsJsonAsync($"/api/players/{playerId}", update);
        putResponse.EnsureSuccessStatusCode();

        var updated = await TestHelper.GetPlayer(playerId.Value, _client);
        updated.Name.Should().Be("Luciana");
        updated.Surname.Should().Be("Martínez");
        updated.Skill.Should().Be(75);
    }

    [Fact]
    public async Task Put_ShouldChangeGenderAndCreateNewType()
    {
        // Arrange
        var create = new CreatePlayerRequestDto
        {
            Name = "Pedro",
            Surname = "Cambio",
            Gender = Gender.Male,
            Skill = 80,
            Strength = 85,
            Speed = 90
        };

        var playerId = await TestHelper.CreateAndGetPlayerIdAsync(create, _client);

        var update = new UpdatePlayerRequestDto
        {
            Gender = Gender.Female,
            ReactionTime = 95
        };

        // Act & Assert
        var putResponse = await _client.PutAsJsonAsync($"/api/players/{playerId}", update);
        putResponse.EnsureSuccessStatusCode();

        var updated = await TestHelper.GetPlayer(playerId.Value, _client);
        updated.Gender.Should().Be(Gender.Female);
        updated.ReactionTime.Should().Be(95);
        updated.Strength.Should().BeNull();
        updated.Speed.Should().BeNull();
    }

    [Fact]
    public async Task Put_ShouldUpdateStrengthAndSpeed_WhenMale()
    {
        // Arrange
        var create = new CreatePlayerRequestDto
        {
            Name = "Luis",
            Surname = "Fuerte",
            Gender = Gender.Male,
            Skill = 70,
            Strength = 70,
            Speed = 70
        };

        var playerId = await TestHelper.CreateAndGetPlayerIdAsync(create, _client);

        var update = new UpdatePlayerRequestDto
        {
            Strength = 85,
            Speed = 88
        };

        // Act & Assert
        var putResponse = await _client.PutAsJsonAsync($"/api/players/{playerId}", update);
        putResponse.EnsureSuccessStatusCode();

        var updated = await TestHelper.GetPlayer(playerId.Value, _client);
        updated.Strength.Should().Be(85);
        updated.Speed.Should().Be(88);
    }

    [Fact]
    public async Task Put_ShouldFail_WhenSwitchingGender_AndMissingKeyFields()
    {
        // Arrange
        var create = new CreatePlayerRequestDto
        {
            Name = "Juan",
            Surname = "Error",
            Gender = Gender.Male,
            Skill = 70,
            Strength = 70,
            Speed = 70
        };

        var playerId = await TestHelper.CreateAndGetPlayerIdAsync(create, _client);

        var update = new UpdatePlayerRequestDto
        {
            Gender = Gender.Female
        };

        // Act & Assert
        var putResponse = await _client.PutAsJsonAsync($"/api/players/{playerId}", update);
        putResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var content = await putResponse.Content.ReadAsStringAsync();
        content.Should().Contain("ReactionTime");
    }

    [Fact]
    public async Task Put_ShouldFail_WhenChangingToFemaleWithoutReactionTime()
    {
        var create = new CreatePlayerRequestDto
        {
            Name = "Carlos",
            Surname = "Invalid",
            Gender = Gender.Male,
            Skill = 80,
            Strength = 80,
            Speed = 80
        };

        var playerId = await TestHelper.CreateAndGetPlayerIdAsync(create, _client);

        var update = new UpdatePlayerRequestDto
        {
            Gender = Gender.Female
        };

        var putResponse = await _client.PutAsJsonAsync($"/api/players/{playerId}", update);
        putResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var content = await putResponse.Content.ReadAsStringAsync();
        content.Should().Contain("ReactionTime");
    }

    [Fact]
    public async Task Delete_ShouldRemovePlayerSuccessfully()
    {
        // Arrange
        var uniqueName = $"Carlos_{Guid.NewGuid():N}[..8]";
        var create = new CreatePlayerRequestDto
        {
            Name = uniqueName,
            Surname = "Borrable",
            Gender = Gender.Male,
            Skill = 70,
            Strength = 80,
            Speed = 80
        };

        var createResponse = await _client.PostAsJsonAsync("/api/players", create);
        createResponse.EnsureSuccessStatusCode();

        var getResponse = await _client.GetAsync($"/api/players?name={uniqueName}&page=1&pageSize=1");
        var paged = await getResponse.Content.ReadFromJsonAsync<PagedResultDto<PlayerDto>>(_jsonSerializerOptions);
        var playerId = paged!.Items.Single().Id;

        // Act
        var deleteResponse = await _client.DeleteAsync($"/api/players/{playerId}");

        // Assert
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var checkResponse = await _client.GetAsync($"/api/players/{playerId}");
        checkResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_ShouldReturnNotFound_WhenPlayerDoesNotExist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var response = await _client.DeleteAsync($"/api/players/{nonExistentId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task PostPlayer_ShouldReturnSpanishValidationMessages_WhenAcceptLanguageIsEs()
    {
        // Arrange
        var dto = new CreatePlayerRequestDto
        {
            Name = "Carlos",
            Surname = "Validez",
            Gender = Gender.Male,
            Skill = 80
        };
        var originalCulture = CultureInfo.DefaultThreadCurrentUICulture;
        try
        {
            _client.DefaultRequestHeaders.AcceptLanguage.Clear();
            _client.DefaultRequestHeaders.AcceptLanguage.ParseAdd("es");

            // Act
            var response = await _client.PostAsJsonAsync("/api/players", dto);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var json = await response.Content.ReadAsStringAsync();
            json.Should().Contain("Se requiere velocidad");
            json.Should().Contain("Se requiere fuerza");
        }
        finally
        {
            CultureInfo.DefaultThreadCurrentUICulture = originalCulture;
        }
    }

    [Fact]
    public async Task PostPlayer_ShouldReturnSpanishValidationMessages_WhenAcceptLanguageIsEn()
    {
        // Arrange
        var dto = new CreatePlayerRequestDto
        {
            Name = "Carlos",
            Surname = "Validez",
            Gender = Gender.Male,
            Skill = 80
        };
        var originalCulture = CultureInfo.DefaultThreadCurrentUICulture;
        try
        {
            _client.DefaultRequestHeaders.AcceptLanguage.Clear();
            _client.DefaultRequestHeaders.AcceptLanguage.ParseAdd("en");

            // Act
            var response = await _client.PostAsJsonAsync("/api/players", dto);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var json = await response.Content.ReadAsStringAsync();
            json.Should().Contain("Speed is required");
            json.Should().Contain("Strength is required");
        }
        finally
        {
            CultureInfo.DefaultThreadCurrentUICulture = originalCulture;
        }
    }

}
