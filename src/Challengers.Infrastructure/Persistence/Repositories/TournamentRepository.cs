using Challengers.Application.Interfaces.Persistence;
using Challengers.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Challengers.Infrastructure.Persistence.Repositories;

public class TournamentRepository(ChallengersDbContext context) : Repository<Tournament>(context), ITournamentRepository
{
    public async Task<Tournament?> GetWithDetailsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Tournaments
            .Include(t => t.Players)
            .Include(t => t.Matches)
            .Include(t => t.Winner)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }
    public async Task<List<Tournament>> GetAllWithDetailsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Tournaments
            .Include(t => t.Players)
            .Include(t => t.Matches)
            .Include(t => t.Winner)
            .ToListAsync(cancellationToken);
    }
}
