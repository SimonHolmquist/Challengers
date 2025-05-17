using Challengers.Application.Interfaces.Persistence;
using MediatR;

namespace Challengers.Application.Features.Players.Commands.DeletePlayer;

public class DeletePlayerHandler(IPlayerRepository repository)
    : IRequestHandler<DeletePlayerCommand>
{
    private readonly IPlayerRepository _repository = repository;

    public async Task<Unit> Handle(DeletePlayerCommand request, CancellationToken cancellationToken)
    {
        var player = await _repository.GetByIdAsync(request.Id, cancellationToken)
                     ?? throw new KeyNotFoundException(FormatMessage(PlayerIdNotFound, request.Id));

        _repository.Delete(player);
        await _repository.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }

    Task IRequestHandler<DeletePlayerCommand>.Handle(DeletePlayerCommand request, CancellationToken cancellationToken)
    {
        return Handle(request, cancellationToken);
    }
}

