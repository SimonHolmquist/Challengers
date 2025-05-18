using Challengers.Application.DTOs;
using MediatR;

namespace Challengers.Application.Features.Players.Queries.GetPlayerById;

public record GetPlayerByIdQuery(Guid Id) : IRequest<PlayerDto>;
