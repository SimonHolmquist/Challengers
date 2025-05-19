using Challengers.Domain.Entities;
using FluentAssertions;
using System.Globalization;

namespace Challengers.UnitTests.Challengers.Domain.Entities;

public class PlayerTests
{
    [Theory]
    [InlineData(-1)]
    [InlineData(101)]
    public void FemalePlayer_ShouldThrow_WhenSkillIsOutOfRange(int invalidSkill)
    {
        // Arrange & Act
        Action act = () => new FemalePlayer("Ana", "Perez", invalidSkill, 80);
        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>()
           .Where(e => e.ParamName == "skill");
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(101)]
    public void FemalePlayer_ShouldThrow_WhenReactionTimeIsOutOfRange(int invalidReaction)
    {
        // Arrange & Act
        Action act = () => new FemalePlayer("Ana", "Perez", 80, invalidReaction);
        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>()
           .Where(e => e.ParamName == "reactionTime");
    }

    [Theory]
    [InlineData(-1, 80)]
    [InlineData(80, -1)]
    [InlineData(101, 80)]
    [InlineData(80, 101)]
    public void MalePlayer_ShouldThrow_WhenStrengthOrSpeedIsOutOfRange(int strength, int speed)
    {
        // Arrange & Act
        Action act = () => new MalePlayer("Juan", "Lopez", 80, strength, speed);
        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Theory]
    [InlineData(null, "LastName")]
    [InlineData("Name", null)]
    [InlineData("", "LastName")]
    [InlineData("Name", "")]
    public void FemalePlayer_ShouldThrow_WhenNameOrLastNameInvalid(string? name, string? lastname)
    {
        // Arrange & Act
        Action act = () => new FemalePlayer(name!, lastname!, 80, 80);
        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void ExplainScore_ShouldIncludeAllComponents_ForFemalePlayer()
    {
        // Arrange
        var player = new FemalePlayer("Ana", "Perez", 80, 70);
        var luck = 0.5;
        var expectedScore = player.GetMatchScore(luck);

        // Act
        var explanation = player.ExplainScore(expectedScore, luck);

        // Assert
        explanation.Should().Contain("80")        // Skill
                   .And.Contain("70")             // ReactionTime
                   .And.Contain("0.5")            // Luck
                   .And.Contain(expectedScore.ToString()); // Final score
    }
    [Fact]
    public void ExplainScore_ShouldIncludeAllComponents_ForMalePlayer()
    {
        // Arrange
        var player = new MalePlayer("Juan", "Gomez", 85, 75, 65);
        var luck = 0.4;
        var expectedScore = player.GetMatchScore(luck);
        
        // Act
        var explanation = player.ExplainScore(expectedScore, luck);

        // Assert
        explanation.Should().Contain("85")       // Skill
                   .And.Contain("75")            // Strength
                   .And.Contain("65")            // Speed
                   .And.Contain("0.4")           // Luck
                   .And.Contain(expectedScore.ToString()); // Final score
    }

    [Fact]
    public void SetName_ShouldUpdatePlayerName()
    {
        // Arrange
        var player = new FemalePlayer("Ana", "Perez", 80, 80);
        // Act
        player.SetName("Laura");
        // Assert   
        player.FirstName.Should().Be("Laura");
    }

    [Fact]
    public void SetLastName_ShouldUpdatePlayerLastName()
    {
        // Arrange
        var player = new FemalePlayer("Ana", "Perez", 80, 80);
        // Act
        player.SetLastName("Gomez");
        // Assert
        player.LastName.Should().Be("Gomez");
    }

    [Fact]
    public void SetSkill_ShouldUpdatePlayerSkill()
    {
        // Arrange
        var player = new FemalePlayer("Ana", "Perez", 80, 80);
        // Act
        player.SetSkill(90);
        // Assert
        player.Skill.Should().Be(90);
    }

    [Fact]
    public void GetFullName_ShouldReturnConcatenatedName()
    {
        // Arrange
        var player = new FemalePlayer("Ana", "Perez", 80, 80);
        // Act
        var fullName = player.FullName;
        // Assert
        fullName.Should().Be("Ana Perez");
    }

}
