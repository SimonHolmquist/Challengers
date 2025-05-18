namespace Challengers.Application.DTOs
{
    public record class CreateTournamentResponseDto
    {
        public Guid TournamentId { get; init; }
        public string Message { get; init; } = default!;
    }
}
