using Challengers.Application.DTOs;
using Challengers.Application.Features.Players.Queries.GetPlayers;
using Challengers.Application.Interfaces.Persistence;
using Challengers.Domain.Entities;
using Challengers.Domain.Enums;
using Challengers.UnitTests.TestHelpers;
using FluentAssertions;
using Moq;

namespace Challengers.UnitTests.Challengers.Application.Features.Players.Queries.GetPlayers;

public class GetPlayersQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnFilteredPlayers_ByGender()
    {
        var players = new List<Player>
        {
            new MalePlayer("Juan", "Pérez", 80, 70, 60).WithRandomId(),
            new FemalePlayer("Ana", "Gomez", 85, 75).WithRandomId(),
            new FemalePlayer("Laura", "Lopez", 90, 80).WithRandomId()
        };

        var femalePlayers = players.Where(p => p.Gender == Gender.Female);
        var pagedResult = new PagedResultDto<Player>
        {
            Items = [.. femalePlayers],
            TotalCount = 2
        };

        var repositoryMock = new Mock<IPlayerRepository>();
        repositoryMock.Setup(r => r.GetFilteredAsync(It.IsAny<GetPlayersQueryDto>(), It.IsAny<CancellationToken>()))
                      .ReturnsAsync([..femalePlayers]);

        var handler = new GetPlayersQueryHandler(repositoryMock.Object);
        var query = new GetPlayersQuery(new GetPlayersQueryDto { Gender = Gender.Female });

        var result = await handler.Handle(query, CancellationToken.None);

        result.Items.Should().HaveCount(2);
        result.Items.All(p => p.Gender == Gender.Female).Should().BeTrue();
    }


    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoPlayersMatch()
    {
        var pagedResult = new PagedResultDto<Player>
        {
            Items = [],
            TotalCount = 0
        };

        var repositoryMock = new Mock<IPlayerRepository>();
        repositoryMock.Setup(r => r.GetFilteredAsync(It.IsAny<GetPlayersQueryDto>(), It.IsAny<CancellationToken>()))
                      .ReturnsAsync([]);

        var handler = new GetPlayersQueryHandler(repositoryMock.Object);
        var query = new GetPlayersQuery(new GetPlayersQueryDto { Name = "NoExiste" });

        var result = await handler.Handle(query, CancellationToken.None);

        result.Items.Should().BeEmpty();
        result.TotalCount.Should().Be(0);
    }


    [Fact]
    public async Task Handle_ShouldMapPlayerCorrectlyToDto()
    {
        var player = new MalePlayer("Carlos", "Diaz", 75, 70, 65).WithRandomId();

        var pagedResult = new PagedResultDto<Player>
        {
            Items = [player],
            TotalCount = 1
        };

        var repositoryMock = new Mock<IPlayerRepository>();
        repositoryMock.Setup(r => r.GetFilteredAsync(It.IsAny<GetPlayersQueryDto>(), It.IsAny<CancellationToken>()))
                      .ReturnsAsync([player]);

        var handler = new GetPlayersQueryHandler(repositoryMock.Object);
        var query = new GetPlayersQuery(new GetPlayersQueryDto());

        var result = await handler.Handle(query, CancellationToken.None);

        result.Items.Should().ContainSingle(p =>
            p.Name == "Carlos" &&
            p.Surname == "Diaz" &&
            p.Skill == 75 &&
            p.Strength == 70 &&
            p.Speed == 65
        );
    }
}
