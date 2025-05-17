using Challengers.Application.DTOs;
using Challengers.Application.Extensions;
using Challengers.Application.Interfaces.Persistence;
using Challengers.Shared.Helpers;
using Challengers.Shared.Resources;
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
            ? throw new KeyNotFoundException(GetMessage(TournamentNotFound))
            : new TournamentResultDto
        {
            Id = tournament.Id,
            Name = tournament.Name,
            Gender = tournament.Gender.ToLocalizedString(),
            CreatedAt = tournament.CreatedAt,
            Winner = tournament.Winner?.Name ?? string.Empty
        };
    }
}
