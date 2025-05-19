using Challengers.Application.Features.Players.Queries.GetPlayerById;
using Challengers.Application.Interfaces.Persistence;
using Challengers.Domain.Entities;
using Challengers.UnitTests.Helpers;
using FluentAssertions;
using Moq;

namespace Challengers.UnitTests.Challengers.Application.Features.Players.Queries.GetPlayerById;

public class GetPlayerByIdHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnPlayerDto_WhenFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        var player = new MalePlayer("Juan", "Pérez", 80, 70, 60).WithId(id);

        var repositoryMock = new Mock<IPlayerRepository>();
        repositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                      .ReturnsAsync(player);

        var handler = new GetPlayerByIdHandler(repositoryMock.Object);
        var query = new GetPlayerByIdQuery(id);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(id);
        result.FirstName.Should().Be("Juan");
        result.Strength.Should().Be(70);
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenPlayerNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();

        var repositoryMock = new Mock<IPlayerRepository>();
        repositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                      .ReturnsAsync((Player?)null);

        var handler = new GetPlayerByIdHandler(repositoryMock.Object);
        var query = new GetPlayerByIdQuery(id);

        // Act
        Func<Task> act = async () => await handler.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
                 .WithMessage($"*{id}*");
    }

}
