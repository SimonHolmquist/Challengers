namespace Challengers.Application.DTOs;

public record class PlayerExcelDto
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public int Gender { get; set; }
    public int? Skill { get; set; }
    public int? Strength { get; set; }
    public int? Speed { get; set; }
    public int? ReactionTime { get; set; }
    public int RowNumber { get; set; }
}

