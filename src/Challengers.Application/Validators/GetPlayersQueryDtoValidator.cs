using Challengers.Application.DTOs;
using Challengers.Domain.Enums;
using FluentValidation;

namespace Challengers.Application.Validators;

public class GetPlayersQueryDtoValidator : PaginationQueryDtoValidator<GetPlayersQueryDto>
{
    public GetPlayersQueryDtoValidator()
    {
        RuleFor(x => x.FirstName).MaximumLength(MaxNameLength).WithMessage(GetMessage(FirstNameTooLong));

        RuleFor(x => x.LastName).MaximumLength(MaxLastNameLength).WithMessage(GetMessage(LastNameTooLong));

        When(x => x.Gender.HasValue, () =>
        {
            RuleFor(x => x.Gender).Must(gender => gender == Gender.Male || gender == Gender.Female).WithMessage(GetMessage(InvalidGender));
        });
    }
}
