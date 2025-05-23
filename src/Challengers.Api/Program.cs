﻿using Challengers.Api.Internal;
using Challengers.Api.Middleware;
using Challengers.Application.Behaviors;
using Challengers.Application.Features.Tournaments.Commands.CreateTournament;
using Challengers.Application.Validators;
using Challengers.Infrastructure.Auth;
using Challengers.Infrastructure.DependencyInjection;
using Challengers.Infrastructure.Persistence;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Globalization;
using System.Text;

namespace Challengers.Api;

public class Program
{
    private static readonly string[] JwtBearerSchemes = ["Bearer"];

    public static void Main(string[] args)
    {
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

        builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        builder.Services.AddChallengersDbContext(builder.Configuration, builder.Environment);

        builder.Services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssemblyContaining<CreateTournamentCommand>());

        builder.Services.AddControllers(options =>
        {
            if (!builder.Environment.IsEnvironment("Testing") || Environment.GetEnvironmentVariable("ENABLE_AUTH") == "true")
            {
                options.Filters.Add(new AuthorizeFilter(new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build()));
            }
        });

        builder.Services.AddInfrastructureServices();
        builder.Services.AddFluentValidationAutoValidation();
        builder.Services.AddValidatorsFromAssemblyContaining<CreateTournamentRequestDtoValidator>();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Challengers", Version = "v1" });

            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter JWT token",
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            };

            c.AddSecurityDefinition("Bearer", securityScheme);
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { securityScheme, JwtBearerSchemes }
            });
        });

        builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

        var jwtSettings = ValidateJwtSettings(builder);

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidateAudience = true,
                ValidAudience = jwtSettings.Audience,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
                ValidateIssuerSigningKey = true
            };
        });

        builder.Services.AddAuthorization();
        builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

        builder.Services.Configure<RequestLocalizationOptions>(options =>
        {
            var supportedCultures = new[] { new CultureInfo("en"), new CultureInfo("es") };

            options.DefaultRequestCulture = new RequestCulture("en");
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
            options.RequestCultureProviders = [new AcceptLanguageHeaderRequestCultureProvider()];
        });

        var app = builder.Build();

        if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Challengers API v1"));
        }

        app.UseHttpsRedirection();
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        app.UseRouting();
        app.UseRequestLocalization();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.MapGet("/version", () => new
        {
            version = VersionInfo.Version,
            commit = VersionInfo.CommitSha
        });

        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ChallengersDbContext>();

            if (db.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
            {
                db.Database.Migrate();
            }
        }

        app.Run();
    }
    private static JwtSettings ValidateJwtSettings(WebApplicationBuilder builder)
    {
        var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>() ?? throw new InvalidOperationException("JwtSettings section is missing.");

        if (string.IsNullOrWhiteSpace(jwtSettings.Key))
            throw new InvalidOperationException("JwtSettings:Key is missing.");

        if (string.IsNullOrWhiteSpace(jwtSettings.Issuer))
            throw new InvalidOperationException("JwtSettings:Issuer is missing.");

        if (string.IsNullOrWhiteSpace(jwtSettings.Audience))
            throw new InvalidOperationException("JwtSettings:Audience is missing.");

        if (jwtSettings.ExpirationMinutes <= 0)
            throw new InvalidOperationException("JwtSettings:ExpirationMinutes must be greater than 0.");

        return jwtSettings;
    }
}