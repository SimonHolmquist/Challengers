using Challengers.Application.DTOs;
using Challengers.Application.Validators;
using FluentAssertions;
using FluentValidation.TestHelper;

namespace Challengers.UnitTests.Challengers.Application.Validators;

public class PaginationQueryDtoValidatorTests
{
    private readonly PaginationQueryDtoValidator<TestPaginationQueryDto> _validator = new();

    [Fact]
    public void Validate_ShouldHaveError_WhenPageIsLessThanOne()
    {
        // Arrange
        var model = new TestPaginationQueryDto { Page = 0 };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Page);
    }

    [Fact]
    public void Validate_ShouldHaveError_WhenPageSizeIsTooSmall()
    {
        // Arrange
        var model = new TestPaginationQueryDto { PageSize = 0 };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PageSize);
    }

    [Fact]
    public void Validate_ShouldHaveError_WhenPageSizeIsTooLarge()
    {
        // Arrange
        var model = new TestPaginationQueryDto { PageSize = 200 };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PageSize);
    }

    [Fact]
    public void Validate_ShouldNotHaveError_WhenPageAndPageSizeAreValid()
    {
        // Arrange
        var model = new TestPaginationQueryDto { Page = 1, PageSize = 50 };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    private record class TestPaginationQueryDto : PaginationQueryDto { }
}