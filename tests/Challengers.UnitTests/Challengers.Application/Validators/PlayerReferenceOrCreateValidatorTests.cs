using Challengers.Application.DTOs;
using Challengers.Application.Validators;
using Challengers.Domain.Enums;
using FluentAssertions;
using FluentValidation.TestHelper;

namespace Challengers.UnitTests.Challengers.Application.Validators;

public class PlayerReferenceOrCreateValidatorTests
{
    private readonly PlayerReferenceOrCreateValidator _validator = new();

    [Fact]
    public void Validate_ShouldHaveError_WhenIdIsEmpty()
    {
        // Arrange
        var model = new PlayerDto { Id = Guid.Empty, Name = "Test" };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Validate_ShouldNotHaveError_WhenIdIsValid()
    {
        // Arrange
        var model = new PlayerDto { Id = Guid.NewGuid() };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_ShouldHaveError_WhenCreatePlayerDataIsInvalid()
    {
        // Arrange
        var model = new PlayerDto
        {
            Name = "",
            Surname = "",
            Skill = 200,
            Gender = Gender.Male,
            Strength = 120,
            Speed = null
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public void Validate_ShouldNotHaveError_WhenCreatePlayerDataIsValid()
    {
        // Arrange
        var model = new PlayerDto
        {
            Name = "Lucía",
            Surname = "Martínez",
            Skill = 80,
            ReactionTime = 85,
            Gender = Gender.Female,
            Strength = null,
            Speed = null
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}