using Challengers.Application.DTOs;
using Challengers.Application.Interfaces.Persistence;
using MediatR;

namespace Challengers.Application.Features.Tournaments.Queries.GetTournamentResult;

public class GetTournamentResultHandler(
    ITournamentRepository tournamentRepository
) : IRequestHandler<GetTournamentResultQuery, TournamentResultDto>
{
    private readonly ITournamentRepository _tournamentRepository = tournamentRepository;

    public async Task<TournamentResultDto> Handle(GetTournamentResultQuery request, CancellationToken cancellationToken)
    {
        var tournament = await _tournamentRepository.GetWithDetailsAsync(request.TournamentId, cancellationToken);

        return tournament is null
            ? throw new KeyNotFoundException(FormatMessage(TournamentNotFound, request.TournamentId))
            : new TournamentResultDto
        {
            Id = tournament.Id,
            Name = tournament.Name,
            Gender = tournament.Gender,
            CreatedAt = tournament.CreatedAt,
            Winner = tournament.Winner?.GetFullName() ?? string.Empty
        };
    }
}
