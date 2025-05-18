using Challengers.Application.DTOs;
using Challengers.Application.Features.Tournaments.Commands.CreateTournament;
using Challengers.Application.Features.Tournaments.Commands.SimulateTournament;
using Challengers.Application.Features.Tournaments.Queries.GetTournamentResult;
using Challengers.Application.Features.Tournaments.Queries.GetTournamentsByFilter;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Challengers.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TournamentsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    public async Task<ActionResult<CreateTournamentResponseDto>> CreateTournament(CreateTournamentRequestDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CreateTournamentCommand(dto), cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.TournamentId }, result);
    }

    [HttpPut("{id}/simulate")]
    public async Task<ActionResult<SimulateTournamentResponseDto>> Simulate(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new SimulateTournamentCommand(id), cancellationToken);
        return Ok(result);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<ActionResult<TournamentResultDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetTournamentResultQuery(id), cancellationToken);
        return Ok(result);
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<List<TournamentResultDto>>> GetAll(
    [FromQuery] GetTournamentsQueryDto dto,
    CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetTournamentsByFilterQuery(dto), cancellationToken);
        return Ok(result);
    }
}
