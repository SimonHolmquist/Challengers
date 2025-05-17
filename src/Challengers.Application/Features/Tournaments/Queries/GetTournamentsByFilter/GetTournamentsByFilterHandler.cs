using Challengers.Application.DTOs;
using Challengers.Application.Interfaces.Persistence;
using MediatR;

namespace Challengers.Application.Features.Tournaments.Queries.GetTournamentsByFilter;

public class GetTournamentsByFilterHandler(ITournamentRepository repository)
    : IRequestHandler<GetTournamentsByFilterQuery, List<TournamentResultDto>>
{
    private readonly ITournamentRepository _repository = repository;

    public async Task<List<TournamentResultDto>> Handle(GetTournamentsByFilterQuery request, CancellationToken cancellationToken)
    {
        var tournaments = await _repository.GetFilteredAsync(request.Dto, cancellationToken);

        return [.. tournaments.Select(t => new TournamentResultDto
        {
            Id = t.Id,
            Name = t.Name,
            Gender = t.Gender,
            CreatedAt = t.CreatedAt,
            Winner = t.Winner?.GetFullName() ?? "-"
        })];

    }
}
