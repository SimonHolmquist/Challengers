using Challengers.Application.DTOs;
using Challengers.Domain.Enums;
using FluentValidation;

namespace Challengers.Application.Validators;

public class GetPlayersQueryDtoValidator : PaginationQueryDtoValidator<GetPlayersQueryDto>
{
    public GetPlayersQueryDtoValidator()
    {
        RuleFor(x => x.Name).MaximumLength(MaxNameLength).WithMessage(NameTooLong);

        RuleFor(x => x.Surname).MaximumLength(MaxSurnameLength).WithMessage(SurnameTooLong);

        When(x => x.Gender.HasValue, () =>
        {
            RuleFor(x => x.Gender).Must(gender => gender == Gender.Male || gender == Gender.Female).WithMessage(InvalidGender);
        });
    }
}
