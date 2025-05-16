using AutoMapper;
using Challengers.Application.DTOs;
using Challengers.Application.Helpers;
using Challengers.Application.Interfaces.Persistence;
using Challengers.Domain.Entities;
using Challengers.Domain.Enums;
using MediatR;

namespace Challengers.Application.Features.Tournaments.Commands.CreateTournament;

public class CreateTournamentHandler(ITournamentRepository tournamentRepository, IMapper mapper) : IRequestHandler<CreateTournamentCommand, CreateTournamentResponseDto>
{
    private readonly ITournamentRepository _tournamentRepository = tournamentRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<CreateTournamentResponseDto> Handle(CreateTournamentCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Dto;

        if (!GenderParser.TryParse(dto.Gender, out var gender))
            throw new ArgumentException(FormatMessage(InvalidGender, Gender_Male, Gender_Female));

        List<Player> players = gender switch
        {
            Gender.Male => [.. dto.Players.Select(_mapper.Map<MalePlayer>).Cast<Player>()],
            Gender.Female => [.. dto.Players.Select(_mapper.Map<FemalePlayer>).Cast<Player>()],
            _ => throw new ArgumentException(FormatMessage(InvalidGender, Gender_Male, Gender_Female))
        };

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
