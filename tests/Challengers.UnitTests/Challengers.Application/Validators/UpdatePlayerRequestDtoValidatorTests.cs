using Challengers.Application.DTOs;
using Challengers.Application.Validators;
using Challengers.Domain.Enums;
using FluentAssertions;
using FluentValidation.TestHelper;
using Xunit;

namespace Challengers.UnitTests.Challengers.Application.Validators;

public class UpdatePlayerRequestDtoValidatorTests
{
    private readonly UpdatePlayerRequestDtoValidator _validator = new();

    [Theory]
    [InlineData(null, 90)]
    [InlineData(85, null)]
    [InlineData(null, null)]
    public void Validate_ShouldHaveError_WhenMaleMissingStrengthOrSpeed(int? strength, int? speed)
    {
        // Arrange
        var model = new UpdatePlayerRequestDto
        {
            Gender = Gender.Male,
            Skill = 80,
            Strength = strength,
            Speed = speed
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        if (strength is null)
            result.ShouldHaveValidationErrorFor(x => x.Strength);
        if (speed is null)
            result.ShouldHaveValidationErrorFor(x => x.Speed);
    }

    [Fact]
    public void Validate_ShouldHaveError_WhenFemaleMissingReactionTime()
    {
        // Arrange
        var model = new UpdatePlayerRequestDto
        {
            Gender = Gender.Female,
            Skill = 80,
            ReactionTime = null
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ReactionTime);
    }

    [Fact]
    public void Validate_ShouldPass_WhenValidMale()
    {
        // Arrange
        var model = new UpdatePlayerRequestDto
        {
            Gender = Gender.Male,
            Skill = 85,
            Strength = 80,
            Speed = 85
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_ShouldPass_WhenValidFemale()
    {
        // Arrange
        var model = new UpdatePlayerRequestDto
        {
            Gender = Gender.Female,
            Skill = 85,
            ReactionTime = 90
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_ShouldPass_WhenGenderIsNull()
    {
        // Arrange
        var model = new UpdatePlayerRequestDto
        {
            Gender = null,
            Skill = 70
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}
