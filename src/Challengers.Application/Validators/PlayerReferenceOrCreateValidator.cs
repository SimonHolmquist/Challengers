using Challengers.Application.DTOs;
using FluentValidation;

namespace Challengers.Application.Validators;

public class PlayerReferenceOrCreateValidator : AbstractValidator<PlayerDto>
{
    public PlayerReferenceOrCreateValidator()
    {
        RuleFor(p => p.Id)
            .Must(id => id.HasValue && id.Value != Guid.Empty)
            .When(p => p.Id is not null)
            .WithMessage(PlayerIdRequired);

        var validator = new CreatePlayerRequestDtoValidator();

        When(p => p.Id is null, () =>
        {
            RuleFor(p => p)
            .Custom((dto, context) =>
        {
            var tempDto = new CreatePlayerRequestDto
            {
                Name = dto.Name ?? string.Empty,
                Surname = dto.Surname ?? string.Empty,
                Skill = dto.Skill ?? MinStat,
                Strength = dto.Strength,
                Speed = dto.Speed,
                ReactionTime = dto.ReactionTime,
                Gender = dto.Gender
            };

            var result = validator.Validate(tempDto);

            foreach (var error in result.Errors)
            {
                context.AddFailure(error.PropertyName, error.ErrorMessage);
            }
        });
        });
    }
}