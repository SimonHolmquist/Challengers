using Challengers.Application.DTOs;
using Challengers.Application.Interfaces.Persistence;
using Challengers.Domain.Entities;
using Challengers.Shared.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Challengers.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlayersController(IPlayerRepository playerRepository) : ControllerBase
{
    private readonly IPlayerRepository _playerRepository = playerRepository;

    [HttpGet("{id}")]
    public async Task<ActionResult<Player>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var player = await _playerRepository.GetByIdAsync(id, cancellationToken);
        return player is null ? NotFound() : Ok(player);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(PlayerDto dto, CancellationToken cancellationToken)
    {
        Player player;

        if (dto.ReactionTime is not null)
        {
            player = new FemalePlayer(dto.Name!, dto.Skill!.Value, dto.ReactionTime.Value);
        }
        else
        {
            player = new MalePlayer(dto.Name!, dto.Skill!.Value, dto.Strength!.Value, dto.Speed!.Value);
        }


        await _playerRepository.AddAsync(player, cancellationToken);
        await _playerRepository.SaveChangesAsync(cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = player.Id }, player.Id);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, PlayerDto dto, CancellationToken cancellationToken)
    {
        var player = await _playerRepository.GetByIdAsync(id, cancellationToken);
        if (player is null) return NotFound();

        if (player is MalePlayer male)
        {
            if (dto.Name is not null) male.SetName(dto.Name);
            if (dto.Skill is not null) male.SetSkill(dto.Skill.Value);
            if (dto.Strength is not null) male.SetStrength(dto.Strength.Value);
            if (dto.Speed is not null) male.SetSpeed(dto.Speed.Value);
        }
        else if (player is FemalePlayer female)
        {
            if (dto.Name is not null) female.SetName(dto.Name);
            if (dto.Skill is not null) female.SetSkill(dto.Skill.Value);
            if (dto.ReactionTime is not null) female.SetReactionTime(dto.ReactionTime.Value);
        }
        else
            return BadRequest(ErrorMessages.InvalidGender());

        await _playerRepository.SaveChangesAsync(cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var player = await _playerRepository.GetByIdAsync(id, cancellationToken);
        if (player is null) return NotFound();

        _playerRepository.Delete(player);
        await _playerRepository.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
}
