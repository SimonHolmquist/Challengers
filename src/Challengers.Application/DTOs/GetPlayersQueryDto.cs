using Challengers.Domain.Enums;

namespace Challengers.Application.DTOs
{
    public record class GetPlayersQueryDto : PaginationQueryDto
    {
        public Gender? Gender { get; init; }
        public string? Name { get; init; }
        public string? Surname { get; init; }
    }
}
