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
            .OrderBy(p => p.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> CountAsync(CancellationToken cancellationToken)
    {
        return await _context.Players.CountAsync(cancellationToken);
    }

    public async Task<PagedResultDto<Player>> GetFilteredAsync(GetPlayersQueryDto dto, CancellationToken cancellationToken)
    {
        var query = _context.Players.AsNoTracking().AsQueryable();

        if (dto.Gender.HasValue)
        {
            query = query.Where(p => p.Gender == dto.Gender);
        }

        if (!string.IsNullOrWhiteSpace(dto.Name))
        {
            query = query.Where(p => p.Name.Contains(dto.Name));
        }

        if (!string.IsNullOrWhiteSpace(dto.Surname))
        {
            query = query.Where(p => p.Surname.Contains(dto.Surname));
        }

        var total = await query.CountAsync(cancellationToken);
        var players = await query
            .Skip(((dto.Page ?? 1) - 1) * (dto.PageSize ?? 20))
            .Take(dto.PageSize ?? 20)
            .ToListAsync(cancellationToken);

        return new PagedResultDto<Player>
        {
            Items = players,
            TotalCount = total,
            Page = dto.Page ?? 1,
            PageSize = dto.PageSize ?? 20
        };
    }
    public async Task ReplaceAsync(Player player, CancellationToken cancellationToken)
    {
        var existingPlayer = await GetByIdAsync(player.Id, cancellationToken);
        if (existingPlayer != null)
        {
            _context.Players.Remove(existingPlayer);
        }
        _context.Players.Add(player);
    }
}
