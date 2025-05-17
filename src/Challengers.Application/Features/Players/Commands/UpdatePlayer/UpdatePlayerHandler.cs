using Challengers.Application.Interfaces.Persistence;
using Challengers.Domain.Entities;
using Challengers.Domain.Enums;
using Challengers.Shared.Helpers;
using MediatR;

namespace Challengers.Application.Features.Players.Commands.UpdatePlayer;

public class UpdatePlayerHandler(IPlayerRepository repository) : IRequestHandler<UpdatePlayerCommand>
{
    private readonly IPlayerRepository _repository = repository;

    public async Task<Unit> Handle(UpdatePlayerCommand request, CancellationToken cancellationToken)
    {
        var id = request.Id;
        var dto = request.Dto;

        var existing = await _repository.GetByIdAsync(id, cancellationToken) ?? throw new KeyNotFoundException(FormatMessage(PlayerIdNotFound, id));

        if (existing.Gender != dto.Gender)
        {
            Player replacement = dto.Gender switch
            {
                Gender.Male => new MalePlayer(dto.Name!, dto.Surname!, dto.Skill!.Value, dto.Strength!.Value, dto.Speed!.Value),
                Gender.Female => new FemalePlayer(dto.Name!, dto.Surname!, dto.Skill!.Value, dto.ReactionTime!.Value),
                _ => throw new ArgumentException(ErrorMessages.InvalidGender())
            };

            replacement.ForceSetId(existing.Id);

            await _repository.ReplaceAsync(replacement, cancellationToken);
        }
        else
        {
            existing.SetName(dto.Name!);
            existing.SetSurname(dto.Surname!);
            existing.SetSkill(dto.Skill!.Value);

            if (existing is MalePlayer m)
            {
                m.SetStrength(dto.Strength!.Value);
                m.SetSpeed(dto.Speed!.Value);
            }
            else if (existing is FemalePlayer f)
            {
                f.SetReactionTime(dto.ReactionTime!.Value);
            }

            _repository.Update(existing);
        }

        await _repository.SaveChangesAsync(cancellationToken);


        return Unit.Value;
    }

    Task IRequestHandler<UpdatePlayerCommand>.Handle(UpdatePlayerCommand request, CancellationToken cancellationToken)
    {
        return Handle(request, cancellationToken);
    }
}
