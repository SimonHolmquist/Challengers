using Challengers.Application.DTOs;
using Challengers.Domain.Common;
using Challengers.Domain.Entities;
using Challengers.Domain.Enums;
using System.Net.Http.Json;
using System.Text.Json;

namespace Challengers.UnitTests.Helpers;

public static class TestHelper
{
    private static readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static T WithId<T>(this T entity, Guid id) where T : Entity<Guid>
    {
        typeof(T).GetProperty(nameof(Entity<Guid>.Id))!
            .SetValue(entity, id);
        return entity;
    }

    public static T WithRandomId<T>(this T entity) where T : Entity<Guid> => entity.WithId(Guid.NewGuid());

    public static List<Player> FakePlayers(bool Male, int count)
    {
        return [.. Enumerable.Range(1, count).Select<int, Player>(i =>
            Male
                ? new MalePlayer($"P{i}", $"M{i}", 80, 70, 60).WithRandomId()
                : new FemalePlayer($"P{i}", $"F{i}", 80, 75).WithRandomId()
        )];
    }

    public static Tournament SimulateAndReturn(this Tournament t)
    {
        t.Simulate(); return t;
    }
    public static List<PlayerDto> GetValidPlayers(int count)
    {
        return [.. Enumerable.Range(1, count)
            .Select(i => new PlayerDto
            {
                FirstName = $"Player{i}",
                LastName = $"LastName{i}",
                Skill = 80,
                Strength = 80,
                Speed = 80,
                Gender = Gender.Male
            })];
    }

    public static CreatePlayerRequestDto GetValidMalePlayer() =>
        new()
        {
            FirstName = "Juan",
            LastName = "Pérez",
            Gender = Gender.Male,
            Skill = 80,
            Strength = 85,
            Speed = 75,
            ReactionTime = null
        };

    public static CreatePlayerRequestDto GetValidFemalePlayer() =>
        new()
        {
            FirstName = "Ana",
            LastName = "Gómez",
            Gender = Gender.Female,
            Skill = 75,
            ReactionTime = 85,
            Strength = null,
            Speed = null
        };

    public static async Task<Guid?> CreateAndGetPlayerIdAsync(CreatePlayerRequestDto dto, HttpClient client)
    {
        var _client = client;
        await _client.PostAsJsonAsync("/api/players", dto);
        var response = await _client.GetAsync($"/api/players?firstname={dto.FirstName}");
        var raw = await response.Content.ReadAsStringAsync();
        var paged = JsonSerializer.Deserialize<PagedResultDto<PlayerDto>>(raw, _jsonSerializerOptions);
        return paged!.Items[0].Id;
    }

    public static async Task<PlayerDto> GetPlayer(Guid id, HttpClient client)
    {
        var _client = client;
        var response = await _client.GetAsync($"/api/players/{id}");
        var raw = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<PlayerDto>(raw, _jsonSerializerOptions)!;
    }

}
