using Challengers.Application.DTOs;
using Challengers.Application.Features.Tournaments.Commands.CreateTournament;
using Challengers.Application.Interfaces.Persistence;
using Challengers.Domain.Entities;
using Challengers.Domain.Enums;
using Challengers.UnitTests.Helpers;
using FluentAssertions;
using Moq;

namespace Challengers.UnitTests.Challengers.Application.Features.Tournaments.Commands.CreateTournament;

public class CreateTournamentHandlerTests
{
    [Fact]
    public async Task Handle_ShouldCreateTournamentWithNewPlayers_WhenSavePlayersIsTrue()
    {
        // Arrange
        var dto = new CreateTournamentRequestDto
        {
            Name = "Open de Córdoba",
            Gender = Gender.Female,
            SavePlayers = true,
            Players =
            [
                new() { FirstName = "Ana", LastName = "López", Skill = 85, ReactionTime = 80, Gender = Gender.Female },
                new() { FirstName = "Laura", LastName = "Pérez", Skill = 88, ReactionTime = 82, Gender = Gender.Female }
            ]
        };

        var tournamentRepo = new Mock<ITournamentRepository>();
        var playerRepo = new Mock<IPlayerRepository>();

        tournamentRepo.Setup(r => r.ExistsByNameAsync(dto.Name, It.IsAny<CancellationToken>()))
                      .ReturnsAsync(false);
        tournamentRepo
            .Setup(r => r.AddAsync(It.IsAny<Tournament>(), It.IsAny<CancellationToken>()))
            .Callback<Tournament, CancellationToken>((t, _) =>
            {
                typeof(Tournament).GetProperty(nameof(Tournament.Id))!
                    .SetValue(t, Guid.NewGuid());
            })
            .Returns(Task.CompletedTask);

        playerRepo
            .Setup(r => r.AddAsync(It.IsAny<Player>(), It.IsAny<CancellationToken>()));

        var handler = new CreateTournamentHandler(tournamentRepo.Object, playerRepo.Object);
        var command = new CreateTournamentCommand(dto);

        // Act
        var response = await handler.Handle(command, CancellationToken.None);

        // Assert
        tournamentRepo.Verify(r => r.AddAsync(It.Is<Tournament>(t => t.Name == dto.Name), default), Times.Once);
        playerRepo.Verify(r => r.AddAsync(It.IsAny<Player>(), default), Times.Exactly(2));
        playerRepo.Verify(r => r.SaveChangesAsync(default), Times.Once);
        tournamentRepo.Verify(r => r.SaveChangesAsync(default), Times.Once);
        response.TournamentId.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Handle_ShouldCreateTournamentUsingExistingPlayers()
    {
        // Arrange
        var existingPlayerIds = new[] { Guid.NewGuid(), Guid.NewGuid() };

        var dto = new CreateTournamentRequestDto
        {
            Name = "Challengers Cup",
            Gender = Gender.Male,
            SavePlayers = false,
            Players = [.. existingPlayerIds.Select(id => new PlayerDto { Id = id, Gender = Gender.Male })]
        };

        var tournamentRepo = new Mock<ITournamentRepository>();
        var playerRepo = new Mock<IPlayerRepository>();

        tournamentRepo.Setup(r => r.ExistsByNameAsync(dto.Name, default)).ReturnsAsync(false);

        foreach (var id in existingPlayerIds)
        {
            var player = new MalePlayer("Juan", "X", 80, 80, 80).WithId(id);
            playerRepo.Setup(r => r.GetByIdAsync(id, default)).ReturnsAsync(player);
        }

        tournamentRepo
            .Setup(r => r.AddAsync(It.IsAny<Tournament>(), default))
            .Callback<Tournament, CancellationToken>((t, _) =>
                typeof(Tournament).GetProperty(nameof(Tournament.Id))!.SetValue(t, Guid.NewGuid()))
            .Returns(Task.CompletedTask);

        var handler = new CreateTournamentHandler(tournamentRepo.Object, playerRepo.Object);
        var command = new CreateTournamentCommand(dto);

        // Act
        var response = await handler.Handle(command, default);

        // Assert
        playerRepo.Verify(r => r.GetByIdAsync(It.IsAny<Guid>(), default), Times.Exactly(2));
        playerRepo.Verify(r => r.AddAsync(It.IsAny<Player>(), default), Times.Never);
        tournamentRepo.Verify(r => r.AddAsync(It.IsAny<Tournament>(), default), Times.Once);
        response.TournamentId.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Handle_ShouldCreateTournamentWithMixedPlayers_FromSinglePlayersList()
    {
        // Arrange
        var existingId = Guid.NewGuid();

        var dto = new CreateTournamentRequestDto
        {
            Name = "Mix Open",
            Gender = Gender.Female,
            SavePlayers = true,
            Players =
        [
            new() { Id = existingId },
            new() { FirstName = "Caro", LastName = "Y", Skill = 85, ReactionTime = 75, Gender = Gender.Female }
        ]
        };

        var tournamentRepo = new Mock<ITournamentRepository>();
        var playerRepo = new Mock<IPlayerRepository>();

        tournamentRepo.Setup(r => r.ExistsByNameAsync(dto.Name, default)).ReturnsAsync(false);

        playerRepo.Setup(r => r.GetByIdAsync(existingId, default))
                  .ReturnsAsync(new FemalePlayer("Ana", "X", 80, 80).WithId(existingId));

        playerRepo.Setup(r => r.AddAsync(It.IsAny<Player>(), It.IsAny<CancellationToken>()))
                  .Callback<Player, CancellationToken>((p, _) => p.WithRandomId())
                  .Returns(Task.CompletedTask);

        tournamentRepo.Setup(r => r.AddAsync(It.IsAny<Tournament>(), default))
                      .Callback<Tournament, CancellationToken>((t, _) =>
                          typeof(Tournament).GetProperty(nameof(Tournament.Id))!.SetValue(t, Guid.NewGuid()))
                      .Returns(Task.CompletedTask);

        var handler = new CreateTournamentHandler(tournamentRepo.Object, playerRepo.Object);
        var command = new CreateTournamentCommand(dto);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        playerRepo.Verify(r => r.GetByIdAsync(existingId, default), Times.Once);
        playerRepo.Verify(r => r.AddAsync(It.IsAny<Player>(), default), Times.Once);
        playerRepo.Verify(r => r.SaveChangesAsync(default), Times.Once);
        tournamentRepo.Verify(r => r.AddAsync(It.IsAny<Tournament>(), default), Times.Once);
        tournamentRepo.Verify(r => r.SaveChangesAsync(default), Times.Once);
        result.TournamentId.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenPlayersHaveMixedGenders()
    {
        // Arrange
        var maleId = Guid.NewGuid();
        var femaleId = Guid.NewGuid();
        var dto = new CreateTournamentRequestDto
        {
            Name = "Torneo Mixto",
            Gender = Gender.Male,
            SavePlayers = false,
            Players =
            [
                new() { Id = maleId },
                new() { Id = femaleId }
            ]
        };

        var playerRepo = new Mock<IPlayerRepository>();
        var tournamentRepo = new Mock<ITournamentRepository>();

        playerRepo.Setup(r => r.GetByIdAsync(maleId, default))
          .ReturnsAsync(new MalePlayer("Juan", "X", 80, 80, 80).WithId(maleId));

        playerRepo.Setup(r => r.GetByIdAsync(femaleId, default))
                  .ReturnsAsync(new FemalePlayer("Ana", "Y", 85, 75).WithId(femaleId));

        var handler = new CreateTournamentHandler(tournamentRepo.Object, playerRepo.Object);
        var command = new CreateTournamentCommand(dto);

        // Act
        Func<Task> act = async () => await handler.Handle(command, default);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
                 .WithMessage("*gender*");
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenReferencedPlayerNotFound()
    {
        // Arrange
        var missingId = Guid.NewGuid();

        var dto = new CreateTournamentRequestDto
        {
            Name = "Con Fantasma",
            Gender = Gender.Female,
            SavePlayers = false,
            Players = [new() { Id = missingId }]
        };

        var playerRepo = new Mock<IPlayerRepository>();
        var tournamentRepo = new Mock<ITournamentRepository>();

        playerRepo.Setup(r => r.GetByIdAsync(missingId, default))
                  .ReturnsAsync((Player?)null);

        var handler = new CreateTournamentHandler(tournamentRepo.Object, playerRepo.Object);
        var command = new CreateTournamentCommand(dto);

        // Act
        Func<Task> act = async () => await handler.Handle(command, default);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
                 .WithMessage($"*{missingId}*");
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenTournamentNameAlreadyExists()
    {
        // Arrange
        var dto = new CreateTournamentRequestDto
        {
            Name = "Córdoba Open",
            Gender = Gender.Female,
            SavePlayers = false,
            Players = [new() { Id = Guid.NewGuid() }]
        };

        var playerRepo = new Mock<IPlayerRepository>();
        var tournamentRepo = new Mock<ITournamentRepository>();

        tournamentRepo.Setup(r => r.ExistsByNameAsync(dto.Name, default))
                      .ReturnsAsync(true);

        var handler = new CreateTournamentHandler(tournamentRepo.Object, playerRepo.Object);
        var command = new CreateTournamentCommand(dto);

        // Act
        Func<Task> act = async () => await handler.Handle(command, default);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
                 .WithMessage("*already exists*");
    }

}
