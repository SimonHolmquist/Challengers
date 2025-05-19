using Challengers.Application.DTOs;
using Challengers.Application.Interfaces.Persistence;
using Challengers.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Challengers.Infrastructure.Persistence.Repositories;

public class PlayerRepository(ChallengersDbContext context)
    : Repository<Player>(context), IPlayerRepository
{
    public async Task<List<Player>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken)
    {
        return await _context.Players
            .OrderBy(p => p.FirstName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> CountAsync(CancellationToken cancellationToken)
    {
        return await _context.Players.CountAsync(cancellationToken);
    }

    public async Task<List<Player>> GetFilteredAsync(GetPlayersQueryDto dto, CancellationToken cancellationToken)
    {
        var query = _context.Players.AsNoTracking().AsQueryable();

        if (dto.Gender.HasValue)
        {
            query = query.Where(p => p.Gender == dto.Gender);
        }

        if (!string.IsNullOrWhiteSpace(dto.FirstName))
        {
            query = query.Where(p => p.FirstName.Contains(dto.FirstName));
        }

        if (!string.IsNullOrWhiteSpace(dto.LastName))
        {
            query = query.Where(p => p.LastName.Contains(dto.LastName));
        }

        var players = await query.ToListAsync(cancellationToken);

        return players;
    }
}
