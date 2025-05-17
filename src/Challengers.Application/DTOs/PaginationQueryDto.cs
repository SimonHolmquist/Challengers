namespace Challengers.Application.DTOs
{
    public abstract record class PaginationQueryDto
    {
        public int? Page { get; init; }
        public int? PageSize { get; init; }
    }

}
