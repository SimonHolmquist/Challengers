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
        var result = await _repository.GetFilteredAsync(request.Dto, cancellationToken);

        return new PagedResultDto<PlayerDto>
        {
            Items = [.. result.Items.Select(p => new PlayerDto
            {
                Id = p.Id,
                Name = p.Name,
                Surname = p.Surname,
                Skill = p.Skill,
                Strength = p is MalePlayer m ? m.Strength : null,
                Speed = p is MalePlayer m2 ? m2.Speed : null,
                ReactionTime = p is FemalePlayer f ? f.ReactionTime : null,
                Gender = p.Gender
            })],
            TotalCount = result.TotalCount,
            Page = result.Page,
            PageSize = result.PageSize
        };
    }

}

