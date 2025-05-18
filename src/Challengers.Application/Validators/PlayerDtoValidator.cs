using Challengers.Application.DTOs;
using FluentValidation;

namespace Challengers.Application.Validators;

public class PlayerDtoValidator : AbstractValidator<PlayerDto>
{
    public PlayerDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(GetMessage(PlayerIdRequired));

        RuleFor(x => x.Name)
            .MaximumLength(MaxNameLength)
            .When(x => !string.IsNullOrWhiteSpace(x.Name))
            .WithMessage(FormatMessage(NameTooLong, MaxNameLength));

        RuleFor(x => x.Surname)
            .MaximumLength(MaxSurnameLength)
            .When(x => !string.IsNullOrWhiteSpace(x.Name))
            .WithMessage(FormatMessage(SurnameTooLong, MaxSurnameLength));

        RuleFor(x => x.Skill)
            .InclusiveBetween(MinStat, MaxStat)
            .When(x => x.Skill.HasValue)
            .WithMessage(FormatMessage(SkillOutOfRange, MinStat, MaxStat));
    }
}