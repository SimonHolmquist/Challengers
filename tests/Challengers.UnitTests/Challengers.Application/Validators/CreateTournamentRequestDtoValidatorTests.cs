using Challengers.Application.DTOs;
using Challengers.Application.Validators;
using Challengers.Domain.Enums;
using Challengers.UnitTests.Helpers;
using FluentAssertions;
using FluentValidation.TestHelper;

namespace Challengers.UnitTests.Challengers.Application.Validators;

public class CreateTournamentRequestDtoValidatorTests
{
    private readonly CreateTournamentRequestDtoValidator _validator = new();

    [Fact]
    public void Validate_ShouldHaveError_WhenNameIsEmpty()
    {
        // Arrange
        var model = new CreateTournamentRequestDto
        {
            Name = "",
            Gender = Gender.Male,
            Players = TestHelper.GetValidPlayers(2)
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Validate_ShouldHaveError_WhenPlayerCountIsNotPowerOfTwo()
    {
        // Arrange
        var model = new CreateTournamentRequestDto
        {
            Name = "US Open",
            Gender = Gender.Female,
            Players = TestHelper.GetValidPlayers(3)
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Players);
    }

    [Fact]
    public void Validate_ShouldHaveError_WhenDuplicatedPlayersById()
    {
        // Arrange
        var duplicatedId = Guid.NewGuid();
        var model = new CreateTournamentRequestDto
        {
            Name = "Roland Garros",
            Gender = Gender.Male,
            Players =
            [
                new() { Id = duplicatedId, FirstName = "A", Skill = 80, Strength = 80, Speed = 80, Gender = Gender.Male },
                new() { Id = duplicatedId, FirstName = "B", Skill = 85, Strength = 85, Speed = 85, Gender = Gender.Male }
            ]
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Players);
    }

    [Fact]
    public void Validate_ShouldNotHaveError_WhenModelIsValid()
    {
        // Arrange
        var model = new CreateTournamentRequestDto
        {
            Name = "Challengers Cup",
            Gender = Gender.Female,
            Players =
            [
                new() { FirstName = "Ana", LastName = "One", Skill = 80, ReactionTime = 85, Gender = Gender.Female },
                new() { FirstName = "Laura", LastName = "Two", Skill = 75, ReactionTime = 80, Gender = Gender.Female }
            ]
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}
