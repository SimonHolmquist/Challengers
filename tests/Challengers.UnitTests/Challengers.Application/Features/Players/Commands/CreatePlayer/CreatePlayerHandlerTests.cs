using Challengers.Application.DTOs;
using Challengers.Application.Features.Players.Commands.CreatePlayer;
using Challengers.Application.Interfaces.Persistence;
using Challengers.Domain.Entities;
using Challengers.Domain.Enums;
using Challengers.UnitTests.Helpers;
using FluentAssertions;
using Moq;

namespace Challengers.UnitTests.Challengers.Application.Features.Players.Commands.CreatePlayer
{
    public class CreatePlayerHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldCreateMalePlayerCorrectly()
        {
            // Arrange
            var dto = new CreatePlayerRequestDto
            {
                Name = "Juan",
                Surname = "Pérez",
                Gender = Gender.Male,
                Skill = 80,
                Strength = 85,
                Speed = 70
            };

            var repositoryMock = new Mock<IPlayerRepository>();
            repositoryMock
                .Setup(r => r.AddAsync(It.IsAny<MalePlayer>(), It.IsAny<CancellationToken>()))
                .Callback<Player, CancellationToken>((p, _) => p.WithRandomId());

            var handler = new CreatePlayerHandler(repositoryMock.Object);
            var command = new CreatePlayerCommand(dto);

            // Act
            var id = await handler.Handle(command, CancellationToken.None);

            // Assert
            repositoryMock.Verify(r => r.AddAsync(It.IsAny<MalePlayer>(), It.IsAny<CancellationToken>()), Times.Once);
            repositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            id.Should().NotBeEmpty();
        }

        [Fact]
        public async Task Handle_ShouldCreateFemalePlayerCorrectly()
        {
            // Arrange
            var dto = new CreatePlayerRequestDto
            {
                Name = "Ana",
                Surname = "Lopez",
                Gender = Gender.Female,
                Skill = 90,
                ReactionTime = 85
            };

            var repositoryMock = new Mock<IPlayerRepository>();
            repositoryMock
                .Setup(r => r.AddAsync(It.IsAny<FemalePlayer>(), It.IsAny<CancellationToken>()))
                .Callback<Player, CancellationToken>((p, _) => p.WithRandomId());

            var handler = new CreatePlayerHandler(repositoryMock.Object);
            var command = new CreatePlayerCommand(dto);

            // Act
            var id = await handler.Handle(command, CancellationToken.None);

            // Assert
            repositoryMock.Verify(r => r.AddAsync(It.IsAny<FemalePlayer>(), It.IsAny<CancellationToken>()), Times.Once);
            repositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            id.Should().NotBeEmpty();
        }

        [Fact]
        public async Task Handle_ShouldThrow_WhenGenderIsInvalid()
        {
            // Arrange
            var dto = new CreatePlayerRequestDto
            {
                Name = "Alex",
                Surname = "Test",
                Gender = (Gender)99,
                Skill = 80
            };

            var repositoryMock = new Mock<IPlayerRepository>();
            repositoryMock
                .Setup(r => r.AddAsync(It.IsAny<MalePlayer>(), It.IsAny<CancellationToken>()))
                .Callback<Player, CancellationToken>((p, _) => p.WithRandomId());

            var handler = new CreatePlayerHandler(repositoryMock.Object);
            var command = new CreatePlayerCommand(dto);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                     .WithMessage("*Invalid*");
        }

    }
}
