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
            Name = new string('A', 101)
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Validate_ShouldHaveError_WhenSurnameTooLong()
    {
        // Arrange
        var model = new GetPlayersQueryDto
        {
            Surname = new string('B', 101)
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Surname);
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
            Name = "Ana",
            Surname = "Gomez",
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