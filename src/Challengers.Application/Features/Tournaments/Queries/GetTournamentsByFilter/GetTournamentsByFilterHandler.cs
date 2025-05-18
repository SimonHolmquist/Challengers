using Challengers.Application.DTOs;
using Challengers.Application.Interfaces.Persistence;
using MediatR;
using System.Linq;

namespace Challengers.Application.Features.Tournaments.Queries.GetTournamentsByFilter;

public class GetTournamentsByFilterHandler(ITournamentRepository repository) : IRequestHandler<GetTournamentsByFilterQuery, PagedResultDto<TournamentResultDto>>
{
    private readonly ITournamentRepository _repository = repository;

    public async Task<PagedResultDto<TournamentResultDto>> Handle(
    GetTournamentsByFilterQuery request,
    CancellationToken cancellationToken)
    {
        var tournaments = await _repository.GetFilteredAsync(request.Dto, cancellationToken);

        var totalCount = tournaments.Count;

        var page = request.Dto.Page ?? DefaultPage;
        var pageSize = request.Dto.PageSize ?? DefaultPageSize;

        var pagedItems = tournaments
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(t => new TournamentResultDto
            {
                Id = t.Id,
                Name = t.Name,
                Gender = t.Gender,
                CreatedAt = t.CreatedAt,
                Winner = t.Winner?.GetFullName() ?? string.Empty
            })
            .ToList();

        return new PagedResultDto<TournamentResultDto>
        {
            TotalCount = totalCount,
            Items = pagedItems
        };
    }

}
