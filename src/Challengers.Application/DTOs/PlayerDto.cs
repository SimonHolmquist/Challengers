namespace Challengers.Application.DTOs;

public record class PlayerDto
{
    public string Name { get; init; } = default!;
    public int Skill { get; init; }
    public int? Strength { get; init; }
    public int? Speed { get; init; }
    public int? ReactionTime { get; init; }
}
