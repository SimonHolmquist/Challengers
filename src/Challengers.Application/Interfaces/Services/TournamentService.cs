using AutoMapper;
using Challengers.Application.DTOs;

namespace Challengers.Application.Interfaces.Services
{
    public class TournamentService(IMapper mapper) : ITournamentService
    {
        private readonly IMapper _mapper = mapper;

        public async Task<CreateTournamentResponseDto> CreateTournamentAsync(CreateTournamentRequestDto dto, CancellationToken cancellationToken = default)
        {
            // Implementación pendiente
            throw new NotImplementedException();
        }

        public async Task<SimulateTournamentResponseDto> SimulateTournamentAsync(Guid tournamentId, CancellationToken cancellationToken = default)
        {
            // Implementación pendiente
            throw new NotImplementedException();
        }

        public async Task<TournamentResultDto> GetTournamentResultAsync(Guid tournamentId, CancellationToken cancellationToken = default)
        {
            // Implementación pendiente
            throw new NotImplementedException();
        }
    }
}
