using Challengers.Application.DTOs;
using Challengers.Application.Interfaces.Persistence;
using Challengers.Domain.Entities;
using MediatR;

namespace Challengers.Application.Features.Players.Queries.GetPlayers;

public class GetPlayersQueryHandler(IPlayerRepository repository) : IRequestHandler<GetPlayersQuery, PagedResultDto<PlayerDto>>
{
    private readonly IPlayerRepository _repository = repository;

    public async Task<PagedResultDto<PlayerDto>> Handle(GetPlayersQuery request, CancellationToken cancellationToken)
    {
        var players = await _repository.GetFilteredAsync(request.Dto, cancellationToken);

        var totalCount = players.Count;

        var page = request.Dto.Page ?? DefaultPage;
        var pageSize = request.Dto.PageSize ?? DefaultPageSize;

        var pagedItems = players
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new PlayerDto
            {
                Id = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName,
                Skill = p.Skill,
                Strength = p is MalePlayer m ? m.Strength : null,
                Speed = p is MalePlayer m2 ? m2.Speed : null,
                ReactionTime = p is FemalePlayer f ? f.ReactionTime : null,
                Gender = p.Gender
            })
            .ToList();

        return new PagedResultDto<PlayerDto>
        {
            TotalCount = totalCount,
            Items = pagedItems
        };
    }

}

