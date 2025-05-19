using Challengers.Domain.Enums;

namespace Challengers.Application.DTOs;

public record class PlayerDto
{
    public Guid? Id { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? FullName { get => $"{FirstName} {LastName}";}
    public int? Skill { get; init; }
    public int? Strength { get; init; }
    public int? Speed { get; init; }
    public int? ReactionTime { get; init; }
    public Gender Gender { get; init; }
}
