using Challengers.Shared.Interfaces.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Challengers.Infrastructure.Persistence.Repositories;

public class Repository<T>(ChallengersDbContext context) : IRepository<T> where T : class
{
    protected readonly ChallengersDbContext _context = context;

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        => await _context.Set<T>().AddAsync(entity, cancellationToken);

    public void Update(T entity) => _context.Set<T>().Update(entity);

    public void Delete(T entity) => _context.Set<T>().Remove(entity);

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.Set<T>().FindAsync([id], cancellationToken);

    public async Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.Set<T>().ToListAsync(cancellationToken);

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken);
}
