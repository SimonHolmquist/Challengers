using Challengers.Application.DTOs;
using Challengers.Application.Interfaces.Persistence;
using Challengers.Domain.Entities;
using Challengers.Shared.Helpers;
using Challengers.Shared.Resources;
using MediatR;

namespace Challengers.Application.Features.Tournaments.Commands.SimulateTournament;

public class SimulateTournamentHandler(
    ITournamentRepository tournamentRepository
) : IRequestHandler<SimulateTournamentCommand, SimulateTournamentResponseDto>
{
    private readonly ITournamentRepository _tournamentRepository = tournamentRepository;

    public async Task<SimulateTournamentResponseDto> Handle(SimulateTournamentCommand request, CancellationToken cancellationToken)
    {
        var tournament = await _tournamentRepository.GetWithDetailsAsync(request.TournamentId, cancellationToken) ?? throw new KeyNotFoundException(GetMessage(TournamentNotFound));
        tournament.Simulate();

        _tournamentRepository.Update(tournament);
        await _tournamentRepository.SaveChangesAsync(cancellationToken);

        return new SimulateTournamentResponseDto
        {
            TournamentId = tournament.Id,
            Winner = tournament.Winner?.Name ?? string.Empty
        };
    }
}
