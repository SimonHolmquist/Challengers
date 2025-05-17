using Challengers.Application.DTOs;
using MediatR;

namespace Challengers.Application.Features.Players.Commands.UpdatePlayer;
public record UpdatePlayerCommand(Guid Id, UpdatePlayerRequestDto Dto) : IRequest;
