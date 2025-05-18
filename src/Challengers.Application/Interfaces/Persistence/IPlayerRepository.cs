using Challengers.Application.DTOs;
using Challengers.Domain.Entities;
using Challengers.Shared.Interfaces.Persistence;

namespace Challengers.Application.Interfaces.Persistence;

public interface IPlayerRepository : IRepository<Player>
{
    Task<List<Player>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken);
    Task<int> CountAsync(CancellationToken cancellationToken);
    Task<List<Player>> GetFilteredAsync(GetPlayersQueryDto dto, CancellationToken cancellationToken);
}
