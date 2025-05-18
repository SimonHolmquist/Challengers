using Challengers.Domain.Common;
using Challengers.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Challengers.Infrastructure.Persistence;

public class ChallengersDbContext(DbContextOptions<ChallengersDbContext> options) : DbContext(options)
{
    public DbSet<Tournament> Tournaments => Set<Tournament>();
    public DbSet<Match> Matches => Set<Match>();
    public DbSet<Player> Players => Set<Player>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<Entity<Guid>>())
        {
            if (entry.State == EntityState.Modified)
                entry.Entity.SetModified();
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
