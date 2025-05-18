using Challengers.Application.Validators;
using Challengers.UnitTests.Helpers;
using FluentAssertions;
using FluentValidation.TestHelper;

namespace Challengers.UnitTests.Challengers.Application.Validators;

public class CreatePlayerRequestDtoValidatorTests
{
    private readonly CreatePlayerRequestDtoValidator _validator = new();

    [Fact]
    public void Validate_ShouldHaveError_WhenNameIsEmpty()
    {
        // Arrange
        var model = TestHelper.GetValidMalePlayer() with { Name = "" };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Validate_ShouldHaveError_WhenSkillIsOutOfRange()
    {
        // Arrange
        var model = TestHelper.GetValidFemalePlayer() with { Skill = 150 };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Skill);
    }

    [Fact]
    public void Validate_ShouldHaveError_WhenFemalePlayerIsMissingReactionTime()
    {
        // Arrange
        var model = TestHelper.GetValidFemalePlayer() with { ReactionTime = null };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ReactionTime);
    }

    [Fact]
    public void Validate_ShouldHaveError_WhenMalePlayerHasReactionTime()
    {
        // Arrange
        var model = TestHelper.GetValidMalePlayer() with { ReactionTime = 60 };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ReactionTime);
    }

    [Fact]
    public void Validate_ShouldNotHaveError_WhenMalePlayerIsValid()
    {
        // Arrange
        var model = TestHelper.GetValidMalePlayer();

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_ShouldNotHaveError_WhenFemalePlayerIsValid()
    {
        // Arrange
        var model = TestHelper.GetValidFemalePlayer();

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}