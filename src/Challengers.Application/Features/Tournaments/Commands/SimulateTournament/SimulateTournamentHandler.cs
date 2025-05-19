using Challengers.Application.DTOs;
using Challengers.Application.Interfaces.Persistence;
using MediatR;

namespace Challengers.Application.Features.Tournaments.Commands.SimulateTournament;

public class SimulateTournamentHandler(
    ITournamentRepository tournamentRepository
) : IRequestHandler<SimulateTournamentCommand, SimulateTournamentResponseDto>
{
    private readonly ITournamentRepository _tournamentRepository = tournamentRepository;

    public async Task<SimulateTournamentResponseDto> Handle(SimulateTournamentCommand request, CancellationToken cancellationToken)
    {
        var tournament = await _tournamentRepository.GetWithDetailsAsync(request.TournamentId, cancellationToken) ?? throw new KeyNotFoundException(FormatMessage(TournamentNotFound, request.TournamentId));
        
        if (tournament.Winner is not null || tournament.Matches.Count != 0)
        {
            throw new ArgumentException(GetMessage(TournamentAlreadyCompleted));
        }

        tournament.Simulate();

        await _tournamentRepository.SaveChangesAsync(cancellationToken);


        return new SimulateTournamentResponseDto
        {
            TournamentId = tournament.Id,
            Winner = tournament.Winner?.FullName ?? string.Empty
        };
    }
}
