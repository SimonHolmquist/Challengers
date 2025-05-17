using Challengers.Application.DTOs;
using FluentValidation;

namespace Challengers.Application.Validators;

public class PaginationQueryDtoValidator<T> : AbstractValidator<T> where T : PaginationQueryDto
{
    public PaginationQueryDtoValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1)
            .When(x => x.Page.HasValue);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .When(x => x.PageSize.HasValue);
    }
}

