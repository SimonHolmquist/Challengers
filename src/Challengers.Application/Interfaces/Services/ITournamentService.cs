using Challengers.Application.DTOs;

namespace Challengers.Application.Interfaces.Services;

public interface ITournamentService
{
    Task<CreateTournamentResponseDto> CreateTournamentAsync(CreateTournamentRequestDto dto, CancellationToken cancellationToken = default);
    Task<SimulateTournamentResponseDto> SimulateTournamentAsync(Guid tournamentId, CancellationToken cancellationToken = default);
    Task<TournamentResultDto> GetTournamentResultAsync(Guid tournamentId, CancellationToken cancellationToken = default);
}