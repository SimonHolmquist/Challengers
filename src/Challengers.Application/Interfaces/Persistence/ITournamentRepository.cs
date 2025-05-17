using Challengers.Application.DTOs;
using Challengers.Domain.Entities;
using Challengers.Shared.Interfaces.Persistence;

namespace Challengers.Application.Interfaces.Persistence;

public interface ITournamentRepository : IRepository<Tournament>
{
    Task<Tournament?> GetWithDetailsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<Tournament>> GetAllWithDetailsAsync(CancellationToken cancellationToken = default);
    Task<List<Tournament>> GetFilteredAsync(GetTournamentsQueryDto dto, CancellationToken cancellationToken);
    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken);
}
