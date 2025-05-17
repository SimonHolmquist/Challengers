using Challengers.Application.DTOs;
using Challengers.Domain.Enums;
using FluentValidation;

namespace Challengers.Application.Validators;

public class GetTournamentsQueryDtoValidator : PaginationQueryDtoValidator<GetTournamentsQueryDto>
{
    public GetTournamentsQueryDtoValidator()
    {
        When(x => x.Gender.HasValue, () =>
        {
            RuleFor(x => x.Gender).Must(gender => gender == Gender.Male || gender == Gender.Female).WithMessage(InvalidGender);
        });

        RuleFor(x => x.Name)
            .MaximumLength(TournamentNameMaxLength)
            .WithMessage(FormatMessage(TournamentNameTooLong, TournamentNameMaxLength));
    }
}