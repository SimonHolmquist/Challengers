using Challengers.Application.DTOs;
using MediatR;

namespace Challengers.Application.Features.Tournaments.Queries.GetTournamentsByFilter;

public record GetTournamentsByFilterQuery(GetTournamentsQueryDto Dto) : IRequest<PagedResultDto<TournamentResultDto>>;
