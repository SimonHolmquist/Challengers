using Challengers.Domain.Enums;

namespace Challengers.Application.DTOs;

public record class UpdatePlayerRequestDto
{
    public string? Name { get; init; }
    public string? Surname { get; init; }
    public int? Skill { get; init; }
    public int? Strength { get; init; }
    public int? Speed { get; init; }
    public int? ReactionTime { get; init; }
    public Gender? Gender { get; init; }
}

