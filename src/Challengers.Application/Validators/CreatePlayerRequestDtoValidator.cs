using Challengers.Application.DTOs;
using Challengers.Domain.Enums;
using FluentValidation;

namespace Challengers.Application.Validators;

public class CreatePlayerRequestDtoValidator : AbstractValidator<CreatePlayerRequestDto>
{
    public CreatePlayerRequestDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(NameRequired)
            .MaximumLength(MaxNameLength).WithMessage(NameTooLong);

        RuleFor(x => x.Surname)
            .NotEmpty().WithMessage(SurnameRequired)
            .MaximumLength(MaxSurnameLength).WithMessage(SurnameTooLong);

        RuleFor(x => x.Skill)
            .InclusiveBetween(MinStat, MaxStat)
            .WithMessage(SkillOutOfRange);

        When(x => x.Gender == Gender.Female, () =>
        {
            RuleFor(x => x.ReactionTime)
                .NotNull().WithMessage(ReactionTimeRequired)
                .InclusiveBetween(MinStat, MaxStat)
                .WithMessage(ReactionOutOfRange);

            RuleFor(x => x.Strength)
                .Null().WithMessage(StrengthNotAllowedForFemale);

            RuleFor(x => x.Speed)
                .Null().WithMessage(SpeedNotAllowedForFemale);
        });

        When(x => x.Gender == Gender.Male, () =>
        {
            RuleFor(x => x.Strength)
                .NotNull().WithMessage(StrengthRequiredForMale)
                .InclusiveBetween(MinStat, MaxStat)
                .WithMessage(StrengthOutOfRange);

            RuleFor(x => x.Speed)
                .NotNull().WithMessage(SpeedRequiredForMale)
                .InclusiveBetween(MinStat, MaxStat)
                .WithMessage(SpeedOutOfRange);

            RuleFor(x => x.ReactionTime)
                .Null().WithMessage(ReactionTimeNotAllowedForMale);
        });
    }
}
