namespace Challengers.Application.DTOs
{
    public record class CreateTournamentRequestDto
    {
        public string Name { get; init; } = default!;
        public string Gender { get; init; } = default!;
        public bool SavePlayers { get; init; } = true;
        public List<PlayerDto> Players { get; init; } = [];
    }
}
