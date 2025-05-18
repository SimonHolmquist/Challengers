using Challengers.Application.Interfaces.Persistence;
using Challengers.Infrastructure.Auth;
using Challengers.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Challengers.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<IPlayerRepository, PlayerRepository>();
        services.AddScoped<ITournamentRepository, TournamentRepository>();
        services.AddScoped<IMatchRepository, MatchRepository>();
        services.AddScoped<JwtTokenGenerator>();

        return services;
    }
}
