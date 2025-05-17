using Challengers.Application.DTOs;
using MediatR;

namespace Challengers.Application.Features.Players.Queries.GetPlayers;
public record GetPlayersQuery(GetPlayersQueryDto Dto) : IRequest<PagedResultDto<PlayerDto>>;

