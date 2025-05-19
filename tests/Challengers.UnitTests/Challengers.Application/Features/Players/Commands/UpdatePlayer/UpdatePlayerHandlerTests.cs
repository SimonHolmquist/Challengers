using Challengers.Application.DTOs;
using Challengers.Application.Features.Players.Commands.UpdatePlayer;
using Challengers.Application.Interfaces.Persistence;
using Challengers.Domain.Entities;
using Challengers.Domain.Enums;
using Challengers.UnitTests.Helpers;
using FluentAssertions;
using Moq;

namespace Challengers.UnitTests.Challengers.Application.Features.Players.Commands.UpdatePlayer
{
    public class UpdatePlayerHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldUpdateMalePlayer_WhenGenderIsUnchanged()
        {
            // Arrange
            var id = Guid.NewGuid();
            var existing = new MalePlayer("Juan", "Pérez", 80, 70, 60);
            existing.WithId(id);

            var dto = new UpdatePlayerRequestDto
            {
                FirstName = "Carlos",
                LastName = "Lopez",
                Skill = 90,
                Strength = 85,
                Speed = 75,
                Gender = Gender.Male
            };

            var repositoryMock = new Mock<IPlayerRepository>();
            repositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(existing);

            var handler = new UpdatePlayerHandler(repositoryMock.Object);
            var command = new UpdatePlayerCommand(id, dto);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            existing.FirstName.Should().Be("Carlos");
            existing.LastName.Should().Be("Lopez");
            existing.Skill.Should().Be(90);
            existing.Strength.Should().Be(85);
            existing.Speed.Should().Be(75);

            repositoryMock.Verify(r => r.Update(existing), Times.Once);
            repositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReplacePlayer_WhenGenderChanges()
        {
            // Arrange
            var id = Guid.NewGuid();
            var existing = new MalePlayer("Juan", "Pérez", 80, 70, 60);
            existing.WithId(id);

            var dto = new UpdatePlayerRequestDto
            {
                FirstName = "Ana",
                LastName = "Lopez",
                Skill = 85,
                ReactionTime = 90,
                Gender = Gender.Female
            };

            var repositoryMock = new Mock<IPlayerRepository>();
            repositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(existing);

            var handler = new UpdatePlayerHandler(repositoryMock.Object);
            var command = new UpdatePlayerCommand(id, dto);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            repositoryMock.Verify(r => r.Delete(It.Is<Player>(p => p == existing)), Times.Once);
            repositoryMock.Verify(r => r.AddAsync(It.Is<FemalePlayer>(p =>
                p.FirstName == "Ana" &&
                p.LastName == "Lopez" &&
                p.Skill == 85 &&
                p.ReactionTime == 90 &&
                p.Id == id
            ), It.IsAny<CancellationToken>()), Times.Once);

            repositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Exactly(2));
        }

        [Fact]
        public async Task Handle_ShouldThrow_WhenPlayerNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            var dto = new UpdatePlayerRequestDto
            {
                FirstName = "Juan",
                LastName = "Pérez",
                Skill = 80,
                Gender = Gender.Male,
                Strength = 70,
                Speed = 60
            };

            var repositoryMock = new Mock<IPlayerRepository>();
            repositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                          .ReturnsAsync((Player?)null);

            var handler = new UpdatePlayerHandler(repositoryMock.Object);
            var command = new UpdatePlayerCommand(id, dto);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                     .WithMessage($"*{id}*");
        }

        [Fact]
        public async Task Handle_ShouldThrow_WhenGenderIsInvalid()
        {
            // Arrange
            var id = Guid.NewGuid();
            var existing = new MalePlayer("Juan", "Pérez", 80, 70, 60);
            existing.WithId(id);

            var dto = new UpdatePlayerRequestDto
            {
                FirstName = "Test",
                LastName = "Test",
                Skill = 70,
                Gender = (Gender)99
            };

            var repositoryMock = new Mock<IPlayerRepository>();
            repositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(existing);

            var handler = new UpdatePlayerHandler(repositoryMock.Object);
            var command = new UpdatePlayerCommand(id, dto);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                     .WithMessage("*Invalid*");
        }

    }
}
