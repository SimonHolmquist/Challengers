using Challengers.Application.DTOs;
using MediatR;

namespace Challengers.Application.Features.Tournaments.Queries.GetTournamentResult;

public record GetTournamentResultQuery(Guid TournamentId) : IRequest<TournamentResultDto>;
