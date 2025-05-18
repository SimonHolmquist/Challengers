using Challengers.Domain.Entities;
using Challengers.Domain.Enums;
using Challengers.Domain.Services;
using Challengers.UnitTests.Helpers;
using FluentAssertions;
using Moq;
using Match = Challengers.Domain.Entities.Match;

namespace Challengers.UnitTests.Challengers.Domain.Entities;
public class MatchTests
{
    private readonly Mock<IRandomGenerator> _randomMock;

    public MatchTests()
    {
        _randomMock = new Mock<IRandomGenerator>();
    }

    [Fact]
    public void Simulate_ShouldReturnPlayerWithHigherSkill_WhenLuckIsEqual()
    {
        // Arrange
        var player1 = new MalePlayer("Player", "One", 90, 70, 70);
        var player2 = new MalePlayer("Player", "Two", 70, 60, 60).WithRandomId();

        _randomMock.Setup(r => r.NextDouble()).Returns(0.5);

        var match = new Match(player1, player2, _randomMock.Object);

        // Act
        match.Simulate();

        // Assert
        match.Winner.Should().Be(player1);
    }

    [Fact]
    public void Simulate_ShouldReturnUnderdog_WhenLuckIsHighForHim()
    {
        // Arrange
        var player1 = new FemalePlayer("Top", "Player", 90, 90);
        var player2 = new FemalePlayer("Lucky", "Player", 10, 10).WithRandomId();

        _randomMock.SetupSequence(r => r.NextDouble())
            .Returns(0.1)
            .Returns(0.9);

        var match = new Match(player1, player2, _randomMock.Object);

        // Act
        match.Simulate();

        // Assert
        match.Winner.Should().Be(player2);
    }

    [Fact]
    public void Simulate_ShouldSelectWinnerById_WhenScoresAreExactlyEqual()
    {
        // Arrange
        var guid1 = Guid.Parse("00000000-0000-0000-0000-000000000001");
        var guid2 = Guid.Parse("00000000-0000-0000-0000-000000000002");
        var player1 = new FemalePlayer("Same", "Stats", 80, 80).WithId(guid1);
        var player2 = new FemalePlayer("Same", "Stats", 80, 80).WithId(guid2);

        _randomMock.SetupSequence(r => r.NextDouble())
            .Returns(0.5)
            .Returns(0.5);

        var match = new Match(player1, player2, _randomMock.Object);

        // Act
        match.Simulate();

        // Assert
        match.Winner.Should().Be(player1);
    }

    [Fact]
    public void Simulate_ShouldThrow_WhenCalledTwice()
    {
        // Arrange
        var player1 = new FemalePlayer("Ana", "Uno", 80, 80);
        var player2 = new FemalePlayer("Laura", "Dos", 80, 80).WithRandomId();

        var match = new Match(player1, player2, _randomMock.Object);
        match.Simulate();

        // Act
        Action act = () => match.Simulate();

        // Assert
        act.Should().Throw<InvalidOperationException>()
           .WithMessage(GetMessage(MatchAlreadySimulated));
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenPlayer1IsNull()
    {
        // Arrange
        var player2 = new FemalePlayer("Laura", "Dos", 80, 80);

        // Act
        Action act = () => new Match(null!, player2);

        // Assert
        act.Should().Throw<ArgumentException>()
           .WithMessage(GetMessage(MatchPlayersRequired));
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenPlayer2IsNull()
    {
        // Arrange
        var player1 = new FemalePlayer("Ana", "Uno", 80, 80);

        // Act
        Action act = () => new Match(player1, null!);

        // Assert
        act.Should().Throw<ArgumentException>()
           .WithMessage(GetMessage(MatchPlayersRequired));
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenPlayersHaveSameId()
    {
        // Arrange
        var player = new FemalePlayer("Ana", "Uno", 80, 80);

        // Act
        Action act = () => new Match(player, player);

        // Assert
        act.Should().Throw<ArgumentException>()
           .WithMessage(GetMessage(MatchSamePlayer));
    }

    [Fact]
    public void SetTournament_ShouldAssignTournamentToMatch()
    {
        // Arrange
        var player1 = new FemalePlayer("Ana", "1", 80, 80);
        var player2 = new FemalePlayer("Laura", "2", 80, 80).WithRandomId();

        var tournament = new Tournament("Test", Gender.Female, new List<Player> { player1, player2 });

        var match = new Match(player1, player2, _randomMock.Object);
        // Act
        match.SetTournament(tournament);
        // Assert
        match.Tournament.Should().Be(tournament);
        match.TournamentId.Should().Be(tournament.Id);
    }

    [Fact]
    public void SetTournament_ShouldThrow_WhenNull()
    {
        // Arrange
        var player1 = new FemalePlayer("Ana", "1", 80, 80);
        var player2 = new FemalePlayer("Laura", "2", 80, 80).WithRandomId();
        // Act
        var match = new Match(player1, player2, _randomMock.Object);

        Action act = () => match.SetTournament(null!);
        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

}
