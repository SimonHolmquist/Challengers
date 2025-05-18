using Challengers.Application.Features.Tournaments.Commands.SimulateTournament;
using Challengers.Application.Interfaces.Persistence;
using Challengers.Domain.Entities;
using Challengers.Domain.Enums;
using Challengers.UnitTests.Helpers;
using FluentAssertions;
using Moq;
using System.Diagnostics;

namespace Challengers.UnitTests.Challengers.Application.Features.Tournaments.Commands.SimulateTournament;

public class SimulateTournamentHandlerTests
{
    [Fact]
    public async Task Handle_ShouldSimulateTournament_WhenValid()
    {
        // Arrange
        var id = Guid.NewGuid();
        var players = new List<Player>
        {
            new FemalePlayer("Ana", "X", 80, 80).WithRandomId(),
            new FemalePlayer("Laura", "Y", 85, 75).WithRandomId()
        };
        var tournament = new Tournament("Simulable", Gender.Female, players).WithId(id);
        Debug.WriteLine($"tournament id {tournament.Id}, id {id}");
        var repo = new Mock<ITournamentRepository>();
        repo.Setup(r => r.GetWithDetailsAsync(id, default)).ReturnsAsync(tournament);

        var handler = new SimulateTournamentHandler(repo.Object);
        var command = new SimulateTournamentCommand(id);

        // Act
        await handler.Handle(command, default);

        // Assert
        tournament.IsCompleted.Should().BeTrue();
        tournament.Winner.Should().NotBeNull();
        tournament.Matches.Should().NotBeEmpty();
        repo.Verify(r => r.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenTournamentNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        var repo = new Mock<ITournamentRepository>();
        repo.Setup(r => r.GetWithDetailsAsync(id, default)).ReturnsAsync((Tournament?)null);

        var handler = new SimulateTournamentHandler(repo.Object);
        var command = new SimulateTournamentCommand(id);

        // Act
        Func<Task> act = async () => await handler.Handle(command, default);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
                 .WithMessage($"*{id}*");
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenTournamentAlreadySimulated()
    {
        // Arrange
        var id = Guid.NewGuid();
        var players = new List<Player>
        {
            new FemalePlayer("Ana", "X", 80, 80).WithRandomId(),
            new FemalePlayer("Laura", "Y", 85, 75).WithRandomId()
        };
        var tournament = new Tournament("Simulado", Gender.Female, players).WithId(id);
        tournament.Simulate();

        var repo = new Mock<ITournamentRepository>();
        repo.Setup(r => r.GetWithDetailsAsync(id, default)).ReturnsAsync(tournament);

        var handler = new SimulateTournamentHandler(repo.Object);
        var command = new SimulateTournamentCommand(id);

        // Act
        Func<Task> act = async () => await handler.Handle(command, default);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
                 .WithMessage("*already*");
    }

}
