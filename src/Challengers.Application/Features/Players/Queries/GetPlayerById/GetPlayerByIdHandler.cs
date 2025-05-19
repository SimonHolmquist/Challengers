using Challengers.Application.DTOs;
using Challengers.Application.Interfaces.Persistence;
using Challengers.Domain.Entities;
using MediatR;

namespace Challengers.Application.Features.Players.Queries.GetPlayerById;

public class GetPlayerByIdHandler(IPlayerRepository repository)
    : IRequestHandler<GetPlayerByIdQuery, PlayerDto>
{
    private readonly IPlayerRepository _repository = repository;

    public async Task<PlayerDto> Handle(GetPlayerByIdQuery request, CancellationToken cancellationToken)
    {
        var player = await _repository.GetByIdAsync(request.Id, cancellationToken)
                     ?? throw new KeyNotFoundException(FormatMessage(PlayerIdNotFound, request.Id));

        return new PlayerDto
        {
            Id = player.Id,
            FirstName = player.FirstName,
            LastName = player.LastName,
            Skill = player.Skill,
            Strength = player is MalePlayer m ? m.Strength : null,
            Speed = player is MalePlayer m2 ? m2.Speed : null,
            ReactionTime = player is FemalePlayer f ? f.ReactionTime : null,
            Gender = player.Gender
        };
    }
}
