using Challengers.Application.DTOs;
using Challengers.Application.Validators;
using Challengers.Domain.Enums;
using FluentAssertions;
using FluentValidation.TestHelper;

namespace Challengers.UnitTests.Challengers.Application.Validators;

public class PlayerDtoValidatorTests
{
    private readonly PlayerDtoValidator _validator = new();

    [Fact]
    public void Validate_ShouldHaveError_WhenIdIsNull()
    {
        // Arrange
        var model = new PlayerDto { Id = null };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Validate_ShouldHaveError_WhenNameTooLong()
    {
        // Arrange
        var model = new PlayerDto
        {
            Id = Guid.NewGuid(),
            FirstName = new string('A', 101)
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FirstName);
    }

    [Fact]
    public void Validate_ShouldHaveError_WhenLastNameTooLong()
    {
        // Arrange
        var model = new PlayerDto
        {
            Id = Guid.NewGuid(),
            FirstName = "Test",
            LastName = new string('B', 101)
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.LastName);
    }

    [Fact]
    public void Validate_ShouldHaveError_WhenSkillOutOfRange()
    {
        // Arrange
        var model = new PlayerDto
        {
            Id = Guid.NewGuid(),
            Skill = 150
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Skill);
    }

    [Fact]
    public void Validate_ShouldNotHaveError_WhenModelIsValid()
    {
        // Arrange
        var model = new PlayerDto
        {
            Id = Guid.NewGuid(),
            FirstName = "Carlos",
            LastName = "Gomez",
            Skill = 90,
            Gender = Gender.Male
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}