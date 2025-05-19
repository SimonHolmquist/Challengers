using Challengers.Application.DTOs;
using Challengers.Application.Validators;
using Challengers.Domain.Enums;
using FluentAssertions;
using FluentValidation.TestHelper;

namespace Challengers.UnitTests.Challengers.Application.Validators;

public class GetPlayersQueryDtoValidatorTests
{
    private readonly GetPlayersQueryDtoValidator _validator = new();

    [Fact]
    public void Validate_ShouldHaveError_WhenNameTooLong()
    {
        // Arrange
        var model = new GetPlayersQueryDto
        {
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
        var model = new GetPlayersQueryDto
        {
            LastName = new string('B', 101)
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.LastName);
    }

    [Fact]
    public void Validate_ShouldHaveError_WhenGenderIsInvalid()
    {
        // Arrange
        var model = new GetPlayersQueryDto
        {
            Gender = (Gender)99
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Gender);
    }

    [Fact]
    public void Validate_ShouldNotHaveError_WhenModelIsValid()
    {
        // Arrange
        var model = new GetPlayersQueryDto
        {
            FirstName = "Ana",
            LastName = "Gomez",
            Gender = Gender.Female,
            Page = 1,
            PageSize = 10
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}