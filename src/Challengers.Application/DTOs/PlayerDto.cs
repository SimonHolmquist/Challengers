namespace Challengers.Application.DTOs;

public record class PlayerDto
{
    public Guid? Id { get; init; }
    public string? Name { get; init; }
    public int? Skill { get; init; }
    public int? Strength { get; init; }
    public int? Speed { get; init; }
    public int? ReactionTime { get; init; }
}
