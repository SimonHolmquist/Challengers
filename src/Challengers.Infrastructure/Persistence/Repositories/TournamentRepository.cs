using Challengers.Application.DTOs;
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

    public async Task<List<Tournament>> GetFilteredAsync(GetTournamentsQueryDto dto, CancellationToken cancellationToken)
    {
        var query = _context.Tournaments
            .Include(t => t.Matches)
            .ThenInclude(m => m.Player1)
            .Include(t => t.Matches)
            .ThenInclude(m => m.Player2)
            .AsNoTracking()
            .AsQueryable();

        if (dto.Gender.HasValue)
        {
            query = query.Where(t => t.Gender == dto.Gender);
        }

        if (dto.Date.HasValue)
        {
            query = query.Where(t => t.CreatedAt.Date == dto.Date.Value.ToDateTime(new()));
        }

        if (!string.IsNullOrWhiteSpace(dto.Name))
        {
            query = query.Where(t => t.Name.Contains(dto.Name));
        }

        return await query.ToListAsync(cancellationToken);
    }

    public Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken) => _context.Tournaments.AnyAsync(t => t.Name == name, cancellationToken);
}
