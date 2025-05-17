using Challengers.Application.DTOs;
using MediatR;

namespace Challengers.Application.Features.Players.Commands.CreatePlayer;

public record CreatePlayerCommand(CreatePlayerRequestDto Dto) : IRequest<Guid>;

