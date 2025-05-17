using Challengers.Application.Interfaces.Persistence;
using Challengers.Domain.Entities;

namespace Challengers.Infrastructure.Persistence.Repositories;

public class MatchRepository(ChallengersDbContext context)
    : Repository<Match>(context), IMatchRepository
{
}
