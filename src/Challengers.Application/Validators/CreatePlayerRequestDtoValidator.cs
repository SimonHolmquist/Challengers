using Challengers.Application.DTOs;
using Challengers.Domain.Enums;
using FluentValidation;

namespace Challengers.Application.Validators;

public class CreatePlayerRequestDtoValidator : AbstractValidator<CreatePlayerRequestDto>
{
    public CreatePlayerRequestDtoValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage(GetMessage(FirstNameRequired))
            .MaximumLength(MaxNameLength).WithMessage(GetMessage(FirstNameTooLong));

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage(GetMessage(LastNameRequired))
            .MaximumLength(MaxLastNameLength).WithMessage(GetMessage(LastNameTooLong));

        RuleFor(x => x.Skill)
            .InclusiveBetween(MinStat, MaxStat)
            .WithMessage(GetMessage(SkillOutOfRange));

        When(x => x.Gender == Gender.Female, () =>
        {
            RuleFor(x => x.ReactionTime)
                .NotNull().WithMessage(GetMessage(ReactionTimeRequired))
                .InclusiveBetween(MinStat, MaxStat)
                .WithMessage(GetMessage(ReactionOutOfRange));

            RuleFor(x => x.Strength)
                .Null().WithMessage(GetMessage(StrengthNotAllowedForFemale));

            RuleFor(x => x.Speed)
                .Null().WithMessage(GetMessage(SpeedNotAllowedForFemale));
        });

        When(x => x.Gender == Gender.Male, () =>
        {
            RuleFor(x => x.Strength)
                .NotNull().WithMessage(GetMessage(StrengthRequiredForMale))
                .InclusiveBetween(MinStat, MaxStat)
                .WithMessage(GetMessage(StrengthOutOfRange));

            RuleFor(x => x.Speed)
                .NotNull().WithMessage(GetMessage(SpeedRequiredForMale))
                .InclusiveBetween(MinStat, MaxStat)
                .WithMessage(GetMessage(SpeedOutOfRange));

            RuleFor(x => x.ReactionTime)
                .Null().WithMessage(GetMessage(ReactionTimeNotAllowedForMale));
        });
    }
}
