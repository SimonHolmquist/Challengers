using Challengers.Domain.Enums;

namespace Challengers.Application.DTOs
{
    public record class GetPlayersQueryDto : PaginationQueryDto
    {
        public Gender? Gender { get; init; }
        public string? FirstName { get; init; }
        public string? LastName { get; init; }
    }
}
