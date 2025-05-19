using Challengers.Application.DTOs;
using FluentValidation;

namespace Challengers.Application.Validators;

public class PlayerDtoValidator : AbstractValidator<PlayerDto>
{
    public PlayerDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(GetMessage(PlayerIdRequired));

        RuleFor(x => x.FirstName)
            .MaximumLength(MaxNameLength)
            .When(x => !string.IsNullOrWhiteSpace(x.FirstName))
            .WithMessage(FormatMessage(FirstNameTooLong, MaxNameLength));

        RuleFor(x => x.LastName)
            .MaximumLength(MaxLastNameLength)
            .When(x => !string.IsNullOrWhiteSpace(x.FirstName))
            .WithMessage(FormatMessage(LastNameTooLong, MaxLastNameLength));

        RuleFor(x => x.Skill)
            .InclusiveBetween(MinStat, MaxStat)
            .When(x => x.Skill.HasValue)
            .WithMessage(FormatMessage(SkillOutOfRange, MinStat, MaxStat));
    }
}