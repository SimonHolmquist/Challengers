using Challengers.Application.Interfaces.Persistence;
using Challengers.Domain.Entities;
using Challengers.Domain.Enums;
using Challengers.Shared.Helpers;
using MediatR;

namespace Challengers.Application.Features.Players.Commands.CreatePlayer;

public class CreatePlayerHandler(IPlayerRepository repository)
    : IRequestHandler<CreatePlayerCommand, Guid>
{
    private readonly IPlayerRepository _repository = repository;

    public async Task<Guid> Handle(CreatePlayerCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Dto;

        Player player = dto.Gender switch
        {
            Gender.Male => new MalePlayer(dto.FirstName, dto.LastName, dto.Skill, dto.Strength!.Value, dto.Speed!.Value),
            Gender.Female => new FemalePlayer(dto.FirstName, dto.LastName, dto.Skill, dto.ReactionTime!.Value),
            _ => throw new ArgumentException(ErrorMessages.InvalidGender())
        };

        await _repository.AddAsync(player, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        return player.Id;
    }

}

