namespace Challengers.Application.DTOs
{
    public record class CreateTournamentRequestDto
    {
        public string Name { get; init; } = default!;
        public string Gender { get; init; } = default!;
        public List<PlayerDto> Players { get; init; } = [];
    }
}
