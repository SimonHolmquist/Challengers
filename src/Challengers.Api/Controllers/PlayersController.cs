using Challengers.Application.DTOs;
using Challengers.Application.Features.Players.Commands.CreatePlayer;
using Challengers.Application.Features.Players.Commands.DeletePlayer;
using Challengers.Application.Features.Players.Commands.UpdatePlayer;
using Challengers.Application.Features.Players.Queries.GetPlayerById;
using Challengers.Application.Features.Players.Queries.GetPlayers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Challengers.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlayersController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("{id}")]
    public async Task<ActionResult<PlayerDto>> Get(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetPlayerByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create([FromBody] CreatePlayerRequestDto dto, CancellationToken cancellationToken)
    {
        var id = await _mediator.Send(new CreatePlayerCommand(dto), cancellationToken);
        return CreatedAtAction(nameof(Get), new { id }, id);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePlayerRequestDto dto, CancellationToken cancellationToken)
    {
        await _mediator.Send(new UpdatePlayerCommand(id, dto), cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeletePlayerCommand(id), cancellationToken);
        return NoContent();
    }

    [HttpGet]
    public async Task<ActionResult<PagedResultDto<PlayerDto>>> GetAll(
        [FromQuery] GetPlayersQueryDto dto,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetPlayersQuery(dto), cancellationToken);
        return Ok(result);
    }
}