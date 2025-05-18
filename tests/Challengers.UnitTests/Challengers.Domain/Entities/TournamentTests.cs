using Challengers.Domain.Entities;
using Challengers.Domain.Enums;
using Challengers.Domain.Services;
using Challengers.UnitTests.Helpers;
using FluentAssertions;
using Moq;

namespace Challengers.UnitTests.Challengers.Domain.Entities;

public class TournamentTests
{
    private readonly Mock<IRandomGenerator> _randomMock;

    public TournamentTests()
    {
        _randomMock = new Mock<IRandomGenerator>();
    }

    [Fact]
    public void Simulate_ShouldSetWinner_WhenTwoPlayersCompete()
    {
        // Arrange 
        var player1 = new FemalePlayer("Superior", "One", 90, 90);
        var player2 = new FemalePlayer("Inferior", "Two", 60, 60).WithRandomId();

        _randomMock.SetupSequence(r => r.NextDouble())
            .Returns(0.5)
            .Returns(0.4);

        var tournament = new Tournament("Test Tournament", Gender.Female, [player1, player2]);

        // Act
        tournament.Simulate(_randomMock.Object);

        // Assert
        var winner = tournament.Winner;
        winner.Should().Be(player1);
        tournament.IsCompleted.Should().BeTrue();
        tournament.Matches.Should().HaveCount(1);
    }

    [Fact]
    public void Simulate_ShouldCompleteTournament_WhenPlayersAreFour()
    {
        // Arrange
        var players = new List<Player>
        {
            new FemalePlayer("Player", "1", 90, 90),
            new FemalePlayer("Player", "2", 80, 80),
            new FemalePlayer("Player", "3", 70, 70),
            new FemalePlayer("Player", "4", 60, 60),
        };

        for (int i = 0; i < players.Count; i++)
        {
            players[i].WithRandomId();
        }

        _randomMock.SetupSequence(r => r.NextDouble())
            .Returns(0.5).Returns(0.4)
            .Returns(0.3).Returns(0.2)
            .Returns(0.6).Returns(0.4);

        var tournament = new Tournament("Test Tournament", Gender.Female, players);

        // Act
        tournament.Simulate(_randomMock.Object);

        // Assert
        tournament.IsCompleted.Should().BeTrue();
        tournament.Winner.Should().NotBeNull();
        tournament.Matches.Should().HaveCount(3);
    }

    [Fact]
    public void Simulate_ShouldThrowException_WhenAlreadySimulated()
    {
        // Arrange
        var player1 = new FemalePlayer("A", "One", 80, 80);
        var player2 = new FemalePlayer("B", "Two", 80, 80).WithRandomId();

        var tournament = new Tournament("Test Tournament", Gender.Female, [player1, player2]);

        tournament.Simulate(_randomMock.Object);

        // Act
        Action act = () => tournament.Simulate(_randomMock.Object);

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*already*");
    }

    [Fact]
    public void Simulate_ShouldCompleteTournament_WhenPlayersAreEight()
    {
        // Arrange
        var players = new List<Player>
        {
            new MalePlayer("Player", "1", 90, 90, 90),
            new MalePlayer("Player", "2", 85, 85, 85),
            new MalePlayer("Player", "3", 80, 80, 80),
            new MalePlayer("Player", "4", 75, 75, 75),
            new MalePlayer("Player", "5", 70, 70, 70),
            new MalePlayer("Player", "6", 65, 65, 65),
            new MalePlayer("Player", "7", 60, 60, 60),
            new MalePlayer("Player", "8", 55, 55, 55),
        };

        foreach (var player in players)
        {
            typeof(Player).GetProperty(nameof(Player.Id))!
                .SetValue(player, Guid.NewGuid());
        }

        var luckValues = Enumerable.Range(0, 14).Select(_ => 0.5).ToArray();
        _randomMock.SetupRepeatedReturns(14);

        var tournament = new Tournament("Complete Tournament", Gender.Male, players);

        // Act
        tournament.Simulate(_randomMock.Object);

        // Assert
        tournament.IsCompleted.Should().BeTrue();
        tournament.Winner.Should().NotBeNull();
        tournament.Matches.Should().HaveCount(7); // 4 + 2 + 1
    }

    [Fact]
    public void Simulate_ShouldDetermineWinnerBasedOnLuck_WhenPlayersAreEqual()
    {
        // Arrange
        var players = new List<Player>
        {
            new MalePlayer("Player", "1", 80, 80, 80),
            new MalePlayer("Player", "2", 80, 80, 80),
            new MalePlayer("Player", "3", 80, 80, 80),
            new MalePlayer("Player", "4", 80, 80, 80),
            new MalePlayer("Player", "5", 80, 80, 80),
            new MalePlayer("Player", "6", 80, 80, 80),
            new MalePlayer("Player", "7", 80, 80, 80),
            new MalePlayer("Player", "8", 80, 80, 80),
        };

        for (int i = 0; i < players.Count; i++)
        {
            typeof(Player).GetProperty(nameof(Player.Id))!
                .SetValue(players[i], Guid.NewGuid());
        }

        var luckValues = new[]
        {
            0.5, 0.4, // Match 1: P1 wins
            0.6, 0.3, // Match 2: P3 wins
            0.2, 0.7, // Match 3: P4 wins
            0.9, 0.1, // Match 4: P5 wins
            0.8, 0.5, // SF1: P1 vs P3 → P1 wins
            0.6, 0.4, // SF2: P4 vs P5 → P4 wins
            0.9, 0.6  // Final: P1 vs P4 → P1 wins
        };

        _randomMock.SetupReturnsFromList(luckValues);

        var tournament = new Tournament("Luck Tournament", Gender.Male, players);

        // Act
        tournament.Simulate(_randomMock.Object);

        // Assert
        tournament.IsCompleted.Should().BeTrue();
        tournament.Matches.Should().HaveCount(7);
        tournament.Winner.Should().NotBeNull();
        tournament.Winner!.Name.Should().Be("Player");
        tournament.Winner!.Surname.Should().Be("1");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_ShouldThrow_WhenNameIsInvalid(string? name)
    {
        // Arrange
        var players = new List<Player>
        {
            new FemalePlayer("Ana", "1", 80, 80),
            new FemalePlayer("Laura", "2", 80, 80)
        };

        // Act
        Action act = () => new Tournament(name!, Gender.Female, players);

        // Assert
        act.Should().Throw<ArgumentException>()
           .WithMessage("*name*");
    }
    [Fact]
    public void Constructor_ShouldThrow_WhenGenderIsInvalid()
    {
        // Arrange
        var players = new List<Player>
        {
            new FemalePlayer("Ana", "1", 80, 80),
            new FemalePlayer("Laura", "2", 80, 80)
        };

        // Act
        Action act = () => new Tournament("Invalid Gender", (Gender)99, players);

        // Assert
        act.Should().Throw<ArgumentException>()
           .WithMessage("*gender*");
    }
    [Theory]
    [InlineData(0)]
    [InlineData(3)]
    [InlineData(5)]
    public void Constructor_ShouldThrow_WhenPlayerCountIsNotPowerOfTwo(int playerCount)
    {
        // Arrange
        var players = Enumerable.Range(1, playerCount)
            .Select(i => new FemalePlayer($"Player", $"{i}", 80, 80))
            .Cast<Player>()
            .ToList();

        // Act
        Action act = () => new Tournament("Invalid Players", Gender.Female, players);

        // Assert
        act.Should().Throw<ArgumentException>()
           .WithMessage("*power of two*");
    }
    [Fact]
    public void Simulate_ShouldThrow_WhenTournamentAlreadySimulated()
    {
        // Arrange
        var players = new List<Player>
        {
            new FemalePlayer("Ana", "1", 80, 80),
            new FemalePlayer("Laura", "2", 80, 80).WithRandomId()
        };
        var tournament = new Tournament("Torneo", Gender.Female, players);
        tournament.Simulate(_randomMock.Object);

        // Act
        Action act = () => tournament.Simulate(_randomMock.Object);

        // Assert
        act.Should().Throw<InvalidOperationException>()
           .WithMessage("*already*");
    }

    [Theory]
    [InlineData(2, 1)]
    [InlineData(4, 3)]
    [InlineData(8, 7)]
    public void Simulate_ShouldCreateCorrectNumberOfMatches(int playerCount, int expectedMatches)
    {
        // Arrange
        var players = Enumerable.Range(1, playerCount)
            .Select(i => new FemalePlayer("Player", i.ToString(), 80, 80))
            .Cast<Player>()
            .ToList();

        foreach (var p in players)
            typeof(Player).GetProperty(nameof(Player.Id))!.SetValue(p, Guid.NewGuid());

        _randomMock.SetupRepeatedReturns(playerCount - 1 * 2);

        var tournament = new Tournament("Test", Gender.Female, players);
        // Act
        tournament.Simulate(_randomMock.Object);
        // Assert
        tournament.Matches.Should().HaveCount(expectedMatches);
    }

    [Fact]
    public void Simulate_ShouldSelectWinnerFromInitialPlayers()
    {
        // Arrange
        var players = Enumerable.Range(1, 4)
            .Select(i => new MalePlayer("Player", i.ToString(), 80, 80, 80))
            .Cast<Player>()
            .ToList();

        foreach (var p in players)
            typeof(Player).GetProperty(nameof(Player.Id))!.SetValue(p, Guid.NewGuid());

        _randomMock.SetupRepeatedReturns(6); // 3 matches × 2 lucks
        // Act
        var tournament = new Tournament("Test", Gender.Male, players);
        tournament.Simulate(_randomMock.Object);
        // Assert
        tournament.Winner.Should().NotBeNull();
        tournament.Players.Should().Contain(tournament.Winner!);
    }

    [Fact]
    public void Simulate_ShouldIncludeAllPlayersInMatches()
    {
        // Arrange
        var players = Enumerable.Range(1, 4)
            .Select(i => new FemalePlayer("P", i.ToString(), 80, 80))
            .Cast<Player>()
            .ToList();

        foreach (var p in players)
            typeof(Player).GetProperty(nameof(Player.Id))!.SetValue(p, Guid.NewGuid());

        _randomMock.SetupRepeatedReturns(6); // 3 matches × 2

        var tournament = new Tournament("Test", Gender.Female, players);
        // Act
        tournament.Simulate(_randomMock.Object);

        var idsInMatches = tournament.Matches
            .SelectMany(m => new[] { m.Player1Id, m.Player2Id, m.WinnerId!.Value });

        var allPlayerIds = players.Select(p => p.Id);
        // Assert
        idsInMatches.Should().Contain(allPlayerIds);
    }
}
