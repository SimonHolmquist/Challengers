using Challengers.Application.DTOs;
using Challengers.Application.Validators;
using Challengers.Domain.Enums;
using FluentAssertions;
using FluentValidation.TestHelper;

namespace Challengers.UnitTests.Challengers.Application.Validators;

public class GetTournamentsQueryDtoValidatorTests
{
    private readonly GetTournamentsQueryDtoValidator _validator = new();

    [Fact]
    public void Validate_ShouldHaveError_WhenNameTooLong()
    {
        // Arrange
        var model = new GetTournamentsQueryDto
        {
            Name = new string('T', 101)
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Validate_ShouldHaveError_WhenGenderIsInvalid()
    {
        // Arrange
        var model = new GetTournamentsQueryDto
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
        var model = new GetTournamentsQueryDto
        {
            Name = "Torneo Nacional",
            Gender = Gender.Male,
            Date = DateOnly.FromDateTime(DateTime.Today),
            Page = 1,
            PageSize = 10
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}