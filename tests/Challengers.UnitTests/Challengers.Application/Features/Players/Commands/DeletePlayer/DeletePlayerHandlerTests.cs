using Challengers.Application.Features.Players.Commands.DeletePlayer;
using Challengers.Application.Interfaces.Persistence;
using Challengers.Domain.Entities;
using Challengers.UnitTests.Helpers;
using FluentAssertions;
using Moq;

namespace Challengers.UnitTests.Challengers.Application.Features.Players.Commands.DeletePlayer
{
    public class DeletePlayerHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldDeletePlayer_WhenExists()
        {
            // Arrange
            var id = Guid.NewGuid();
            var player = new MalePlayer("Juan", "Pérez", 80, 70, 60).WithId(id);

            var repositoryMock = new Mock<IPlayerRepository>();
            repositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(player);

            var handler = new DeletePlayerHandler(repositoryMock.Object);
            var command = new DeletePlayerCommand(id);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            repositoryMock.Verify(r => r.Delete(player), Times.Once);
            repositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrow_WhenPlayerNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();

            var repositoryMock = new Mock<IPlayerRepository>();
            repositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                          .ReturnsAsync((Player?)null);

            var handler = new DeletePlayerHandler(repositoryMock.Object);
            var command = new DeletePlayerCommand(id);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                     .WithMessage($"*{id}*");
        }

    }
}
