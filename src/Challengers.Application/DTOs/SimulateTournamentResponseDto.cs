namespace Challengers.Application.DTOs
{
    public record class SimulateTournamentResponseDto
    {
        public Guid TournamentId { get; init; }
        public string Winner { get; init; } = default!;
    }
}
