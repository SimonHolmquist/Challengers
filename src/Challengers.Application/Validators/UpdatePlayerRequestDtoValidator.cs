using Challengers.Application.DTOs;
using Challengers.Domain.Enums;
using FluentValidation;

namespace Challengers.Application.Validators;

public class UpdatePlayerRequestDtoValidator : AbstractValidator<UpdatePlayerRequestDto>
{
    public UpdatePlayerRequestDtoValidator()
    {
        RuleFor(x => x.Gender)
            .IsInEnum()
            .When(x => x.Gender.HasValue);

        When(x => x.Gender == Gender.Male, () =>
        {
            RuleFor(x => x.Strength)
                .NotNull()
                .WithMessage(GetMessage(StrengthRequiredForMale));

            RuleFor(x => x.Speed)
                .NotNull()
                .WithMessage(GetMessage(SpeedRequiredForMale));
        });

        When(x => x.Gender == Gender.Female, () =>
        {
            RuleFor(x => x.ReactionTime)
                .NotNull()
                .WithMessage(GetMessage(ReactionTimeRequired));
        });
    }
}