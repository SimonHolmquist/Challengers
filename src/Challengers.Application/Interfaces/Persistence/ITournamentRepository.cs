using Challengers.Domain.Entities;
using Challengers.Shared.Interfaces.Persistence;

namespace Challengers.Application.Interfaces.Persistence;

public interface ITournamentRepository : IRepository<Tournament>
{
    Task<Tournament?> GetWithDetailsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<Tournament>> GetAllWithDetailsAsync(CancellationToken cancellationToken = default);
}
