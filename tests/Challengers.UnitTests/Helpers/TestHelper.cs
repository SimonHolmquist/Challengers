using Challengers.Application.DTOs;
using Challengers.Domain.Common;
using Challengers.Domain.Entities;
using Challengers.Domain.Enums;

namespace Challengers.UnitTests.Helpers;

public static class TestHelper
{
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
                Name = $"Player{i}",
                Surname = $"Surname{i}",
                Skill = 80,
                Strength = 80,
                Speed = 80,
                Gender = Gender.Male
            })];
    }

    public static CreatePlayerRequestDto GetValidMalePlayer() =>
        new()
        {
            Name = "Juan",
            Surname = "Pérez",
            Gender = Gender.Male,
            Skill = 80,
            Strength = 85,
            Speed = 75,
            ReactionTime = null
        };

    public static CreatePlayerRequestDto GetValidFemalePlayer() =>
        new()
        {
            Name = "Ana",
            Surname = "Gómez",
            Gender = Gender.Female,
            Skill = 75,
            ReactionTime = 85,
            Strength = null,
            Speed = null
        };
}
