using Challengers.Domain.Enums;

namespace Challengers.Application.DTOs
{
    public record class GetTournamentsQueryDto : PaginationQueryDto
    {
        public DateOnly? Date { get; init; }
        public Gender? Gender { get; init; }
        public string? Name { get; init; }
    }
}
