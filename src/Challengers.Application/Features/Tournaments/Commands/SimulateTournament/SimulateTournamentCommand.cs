using Challengers.Application.DTOs;
using MediatR;

namespace Challengers.Application.Features.Tournaments.Commands.SimulateTournament;

public record SimulateTournamentCommand(Guid TournamentId) : IRequest<SimulateTournamentResponseDto>;
