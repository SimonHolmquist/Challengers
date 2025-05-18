using Challengers.Application.Interfaces.Persistence;
using Challengers.Infrastructure.Auth;
using Challengers.Infrastructure.Persistence;
using Challengers.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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

    public static IServiceCollection AddChallengersDbContext(this IServiceCollection services, IConfiguration config, IHostEnvironment env)
    {
        if (env.EnvironmentName == "Testing")
        {
            services.AddDbContext<ChallengersDbContext>(options =>
            {
                options.UseInMemoryDatabase("ChallengersTestDb");
            });
        }
        else
        {
            var connectionString = config.GetConnectionString("DefaultConnection");
            services.AddDbContext<ChallengersDbContext>(options =>
            {
                options.UseSqlServer(connectionString, sql => sql.EnableRetryOnFailure());
            });
        }

        return services;
    }

}
