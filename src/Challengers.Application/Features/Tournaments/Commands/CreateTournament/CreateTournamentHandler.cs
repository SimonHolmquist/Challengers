using Challengers.Application.DTOs;
using Challengers.Application.Helpers;
using Challengers.Application.Interfaces.Persistence;
using Challengers.Domain.Entities;
using Challengers.Domain.Enums;
using Challengers.Shared.Helpers;
using MediatR;

namespace Challengers.Application.Features.Tournaments.Commands.CreateTournament;

public class CreateTournamentHandler(ITournamentRepository tournamentRepository, IPlayerRepository playerRepository) : IRequestHandler<CreateTournamentCommand, CreateTournamentResponseDto>
{
    private readonly ITournamentRepository _tournamentRepository = tournamentRepository;
    private readonly IPlayerRepository _playerRepository = playerRepository;

    public async Task<CreateTournamentResponseDto> Handle(CreateTournamentCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Dto;

        if (!GenderParser.TryParse(dto.Gender, out var gender))
            throw new ArgumentException(ErrorMessages.InvalidGender());

        var players = new List<Player>();

        foreach (var playerDto in dto.Players)
        {
            if (playerDto.Id is not null)
            {
                var existingPlayer = await _playerRepository.GetByIdAsync(playerDto.Id.Value, cancellationToken) ?? throw new KeyNotFoundException(FormatMessage(PlayerIdNotFound,playerDto.Id));
                players.Add(existingPlayer);
            }
            else
            {
                Player newPlayer = gender switch
                {
                    Gender.Male => new MalePlayer(
                        playerDto.Name!,
                        playerDto.Skill!.Value,
                        playerDto.Strength!.Value,
                        playerDto.Speed!.Value),

                    Gender.Female => new FemalePlayer(
                        playerDto.Name!,
                        playerDto.Skill!.Value,
                        playerDto.ReactionTime!.Value),

                    _ => throw new ArgumentException(ErrorMessages.InvalidGender())
                };

                if (dto.SavePlayers)
                    await _playerRepository.AddAsync(newPlayer, cancellationToken);

                players.Add(newPlayer);
            }
        }

        if (dto.SavePlayers)
            await _playerRepository.SaveChangesAsync(cancellationToken);

        var tournament = new Tournament(dto.Name, gender, players);

        await _tournamentRepository.AddAsync(tournament, cancellationToken);
        await _tournamentRepository.SaveChangesAsync(cancellationToken);

        return new CreateTournamentResponseDto
        {
            TournamentId = tournament.Id,
            Message = GetMessage(TournamentCreatedSuccessfully)
        };
    }

}
