using Challengers.Application.Interfaces.Persistence;
using Challengers.Domain.Entities;
using Challengers.Domain.Enums;
using Challengers.Shared.Helpers;
using MediatR;
using System;

namespace Challengers.Application.Features.Players.Commands.UpdatePlayer;

public class UpdatePlayerHandler(IPlayerRepository repository) : IRequestHandler<UpdatePlayerCommand>
{
    private readonly IPlayerRepository _repository = repository;

    public async Task<Unit> Handle(UpdatePlayerCommand request, CancellationToken cancellationToken)
    {
        var id = request.Id;
        var dto = request.Dto;

        var existing = await _repository.GetByIdAsync(id, cancellationToken) ?? throw new KeyNotFoundException(FormatMessage(PlayerIdNotFound, id));

        if (dto.Gender.HasValue && existing.Gender != dto.Gender.Value)
        {
            Player replacement = dto.Gender.Value switch
            {
                Gender.Male => new MalePlayer(dto.Name ?? existing.Name, dto.Surname ?? existing.Surname, dto.Skill ?? existing.Skill, dto.Strength!.Value, dto.Speed!.Value),
                Gender.Female => new FemalePlayer(dto.Name ?? existing.Name, dto.Surname ?? existing.Surname, dto.Skill ?? existing.Skill, dto.ReactionTime!.Value),
                _ => throw new ArgumentException(ErrorMessages.InvalidGender())
            };

            typeof(Player).GetProperty(nameof(Player.Id))!.SetValue(replacement, existing.Id);

            _repository.Delete(existing);
            await _repository.SaveChangesAsync(cancellationToken);

            typeof(Player).GetProperty(nameof(Player.Id))!.SetValue(replacement, existing.Id);

            await _repository.AddAsync(replacement, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }

        if (!string.IsNullOrEmpty(dto.Name)) existing.SetName(dto.Name);
        if (!string.IsNullOrEmpty(dto.Surname)) existing.SetSurname(dto.Surname);
        if (dto.Skill.HasValue) existing.SetSkill(dto.Skill.Value);

        if (existing is MalePlayer m && dto.Strength is not null && dto.Speed is not null)
        {
            m.SetStrength(dto.Strength.Value);
            m.SetSpeed(dto.Speed.Value);
        }
        else if (existing is FemalePlayer f && dto.ReactionTime is not null)
        {
            f.SetReactionTime(dto.ReactionTime.Value);
        }

        _repository.Update(existing);

        await _repository.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }

    Task IRequestHandler<UpdatePlayerCommand>.Handle(UpdatePlayerCommand request, CancellationToken cancellationToken)
    {
        return Handle(request, cancellationToken);
    }
}
