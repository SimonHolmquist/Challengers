using Challengers.Application.DTOs;
using MediatR;

namespace Challengers.Application.Features.Tournaments.Commands.CreateTournament;

public record CreateTournamentCommand(CreateTournamentRequestDto Dto) : IRequest<CreateTournamentResponseDto>;
