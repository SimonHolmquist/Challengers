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
}
