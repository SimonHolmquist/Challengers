using Challengers.Application.DTOs;
using Challengers.Application.Extensions;
using Challengers.Application.Helpers;
using Challengers.Application.Interfaces.Persistence;
using MediatR;

namespace Challengers.Application.Features.Tournaments.Queries.GetTournamentsByFilter;

public class GetTournamentsByFilterHandler(ITournamentRepository repository)
    : IRequestHandler<GetTournamentsByFilterQuery, List<TournamentResultDto>>
{
    private readonly ITournamentRepository _repository = repository;

    public async Task<List<TournamentResultDto>> Handle(GetTournamentsByFilterQuery request, CancellationToken cancellationToken)
    {
        var tournaments = await _repository.GetAllAsync(cancellationToken);

        if (request.Date is not null)
        {
            var date = request.Date.Value;
            tournaments = [.. tournaments.Where(t => DateOnly.FromDateTime(t.CreatedAt) == date)];
        }

        if (!string.IsNullOrWhiteSpace(request.Gender) &&
            GenderParser.TryParse(request.Gender, out var gender))
        {
            tournaments = [.. tournaments.Where(t => t.Gender == gender)];
        }

        return [.. tournaments.Select(t => new TournamentResultDto
        {
            Id = t.Id,
            Name = t.Name,
            Gender = t.Gender.ToLocalizedString(),
            CreatedAt = t.CreatedAt,
            Winner = t.Winner?.Name ?? string.Empty
        })];
    }
}
