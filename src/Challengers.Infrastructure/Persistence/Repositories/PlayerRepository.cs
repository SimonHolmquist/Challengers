using Challengers.Application.Interfaces.Persistence;
using Challengers.Domain.Entities;

namespace Challengers.Infrastructure.Persistence.Repositories;

public class PlayerRepository(ChallengersDbContext context)
    : Repository<Player>(context), IPlayerRepository
{ }
