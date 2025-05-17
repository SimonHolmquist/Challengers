using Challengers.Application.Features.Players.Commands.CreatePlayer;
using Challengers.Application.Features.Tournaments.Commands.CreateTournament;
using Challengers.Infrastructure.DependencyInjection;
using Challengers.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using FluentValidation;
using Challengers.Application.Behaviors;
using MediatR;
using Challengers.Api.Middleware;
using FluentValidation.AspNetCore;
using Challengers.Application.Validators;
using Challengers.Api.Swagger;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(80);
});

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

builder.Services.AddDbContext<ChallengersDbContext>(options =>
{
    options.UseSqlServer(connectionString, sql =>
    {
        sql.EnableRetryOnFailure();
    });
});

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblyContaining<CreateTournamentCommand>());

builder.Services.AddControllers();

builder.Services.AddInfrastructureServices();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateTournamentRequestDtoValidator>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SchemaFilter<GenderEnumSchemaFilter>();
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Challengers API", Version = "v1" });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Challengers API v1"));
}

app.UseHttpsRedirection();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseAuthorization();
app.MapControllers();
app.MapGet("/ping", () => Results.Ok("pong"));

if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<ChallengersDbContext>();
    db.Database.Migrate();
}

app.Run();