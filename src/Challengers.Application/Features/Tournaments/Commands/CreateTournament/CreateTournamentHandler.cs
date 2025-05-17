using Challengers.Application.DTOs;
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
        
        if (dto.Gender != Gender.Male && dto.Gender != Gender.Female)
            throw new ArgumentException(ErrorMessages.InvalidGender());

        var alreadyExists = await _tournamentRepository.ExistsByNameAsync(dto.Name, cancellationToken);
        if (alreadyExists)
            throw new ArgumentException(FormatMessage(TournamentAlreadyExists, dto.Name));

        var players = new List<Player>();

        foreach (var playerDto in dto.Players)
        {
            if (playerDto.Id is not null)
            {
                var existingPlayer = await _playerRepository.GetByIdAsync(playerDto.Id.Value, cancellationToken) ?? throw new KeyNotFoundException(FormatMessage(PlayerIdNotFound, playerDto.Id));
                players.Add(existingPlayer);
            }
            else
            {
                Player newPlayer = dto.Gender switch
                {
                    Gender.Male => new MalePlayer(
                        playerDto.Name!,
                        playerDto.Surname!,
                        playerDto.Skill!.Value,
                        playerDto.Strength!.Value,
                        playerDto.Speed!.Value),

                    Gender.Female => new FemalePlayer(
                        playerDto.Name!,
                        playerDto.Surname!,
                        playerDto.Skill!.Value,
                        playerDto.ReactionTime!.Value),

                    _ => throw new ArgumentException(ErrorMessages.InvalidGender())
                };

                if (dto.SavePlayers)
                    await _playerRepository.AddAsync(newPlayer, cancellationToken);

                players.Add(newPlayer);
            }
        }

        var mismatchedPlayers = players.Where(p => p.Gender != dto.Gender).ToList();
        if (mismatchedPlayers.Count != 0)
        {
            var offendingNames = string.Join(", ", mismatchedPlayers.Select(p => p.GetFullName()));
            throw new ArgumentException(FormatMessage(TournamentPlayersGenderMismatch, dto.Gender, offendingNames));
        }

        if (dto.SavePlayers)
            await _playerRepository.SaveChangesAsync(cancellationToken);

        var tournament = new Tournament(dto.Name, dto.Gender, players);

        await _tournamentRepository.AddAsync(tournament, cancellationToken);
        await _tournamentRepository.SaveChangesAsync(cancellationToken);

        return new CreateTournamentResponseDto
        {
            TournamentId = tournament.Id,
            Message = GetMessage(TournamentCreatedSuccessfully)
        };
    }

}
