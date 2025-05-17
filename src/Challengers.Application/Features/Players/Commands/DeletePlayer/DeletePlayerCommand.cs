using MediatR;

namespace Challengers.Application.Features.Players.Commands.DeletePlayer;
public record DeletePlayerCommand(Guid Id) : IRequest;
