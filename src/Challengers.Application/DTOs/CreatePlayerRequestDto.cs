using Challengers.Domain.Enums;

namespace Challengers.Application.DTOs;

public record class CreatePlayerRequestDto
{
    public string Name { get; init; } = default!;
    public string Surname { get; init; } = default!;
    public Gender Gender { get; init; }
    public int Skill { get; init; }
    public int? Strength { get; init; }
    public int? Speed { get; init; }
    public int? ReactionTime { get; init; }
}

