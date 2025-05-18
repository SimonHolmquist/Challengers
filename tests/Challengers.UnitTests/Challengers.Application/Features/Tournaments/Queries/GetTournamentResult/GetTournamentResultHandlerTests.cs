using Challengers.Application.Features.Tournaments.Queries.GetTournamentResult;
using Challengers.Application.Interfaces.Persistence;
using Challengers.Domain.Entities;
using Challengers.Domain.Enums;
using Challengers.UnitTests.TestHelpers;
using FluentAssertions;
using Moq;

namespace Challengers.UnitTests.Challengers.Application.Features.Tournaments.Queries.GetTournamentResult;

public class GetTournamentResultHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnTournamentResult_WhenTournamentExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var player1 = new FemalePlayer("Ana", "López", 85, 80).WithRandomId();
        var player2 = new FemalePlayer("Laura", "Pérez", 88, 82).WithRandomId();
        var tournament = new Tournament("Finalizado", Gender.Female, [player1, player2]).WithId(id);

        tournament.Simulate();

        var repo = new Mock<ITournamentRepository>();
        repo.Setup(r => r.GetWithDetailsAsync(id, default)).ReturnsAsync(tournament);

        var handler = new GetTournamentResultHandler(repo.Object);
        var query = new GetTournamentResultQuery(id);

        // Act
        var result = await handler.Handle(query, default);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(id);
        result.Name.Should().Be("Finalizado");
        result.Gender.Should().Be(Gender.Female);
        result.Winner.Should().Be(tournament.Winner!.GetFullName());
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenTournamentNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();

        var repo = new Mock<ITournamentRepository>();
        repo.Setup(r => r.GetWithDetailsAsync(id, default))
            .ReturnsAsync((Tournament?)null);

        var handler = new GetTournamentResultHandler(repo.Object);
        var query = new GetTournamentResultQuery(id);

        // Act
        Func<Task> act = async () => await handler.Handle(query, default);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
                 .WithMessage($"*{id}*");
    }

}
