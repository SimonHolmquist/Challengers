using Challengers.Application.DTOs;
using Challengers.Application.Features.Tournaments.Queries.GetTournamentsByFilter;
using Challengers.Application.Interfaces.Persistence;
using Challengers.Domain.Entities;
using Challengers.Domain.Enums;
using Challengers.UnitTests.TestHelpers;
using FluentAssertions;
using Moq;

namespace Challengers.UnitTests.Challengers.Application.Features.Tournaments.Queries.GetTournamentsByFilter;

public class GetTournamentsByFilterHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnPagedTournaments_WhenMatchesExist()
    {
        // Arrange
        var tournaments = new List<Tournament>
        {
            new Tournament("Torneo 1", Gender.Male, TestHelper.FakePlayers(Male: true, count: 2)).SimulateAndReturn(),
            new Tournament("Torneo 2", Gender.Male, TestHelper.FakePlayers(Male: true, count: 2)).SimulateAndReturn()
        };
        tournaments.ForEach(t => t.WithRandomId());

        var paged = new PagedResultDto<Tournament>
        {
            Items = tournaments,
            TotalCount = 2
        };

        var repo = new Mock<ITournamentRepository>();
        repo.Setup(r => r.GetFilteredAsync(It.IsAny<GetTournamentsQueryDto>(), default)).ReturnsAsync(tournaments);

        var handler = new GetTournamentsByFilterHandler(repo.Object);
        var query = new GetTournamentsByFilterQuery(new GetTournamentsQueryDto());

        // Act
        var result = await handler.Handle(query, default);

        // Assert
        result.Items.Count.Should().Be(2);
        result.Items.Should().HaveCount(2);
        result.Items.All(t => !string.IsNullOrWhiteSpace(t.Winner)).Should().BeTrue();
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoMatchesExist()
    {
        // Arrange
        var paged = new PagedResultDto<Tournament>
        {
            Items = [],
            TotalCount = 0
        };

        var repo = new Mock<ITournamentRepository>();
        repo.Setup(r => r.GetFilteredAsync(It.IsAny<GetTournamentsQueryDto>(), default)).ReturnsAsync([]);

        var handler = new GetTournamentsByFilterHandler(repo.Object);
        var query = new GetTournamentsByFilterQuery(new GetTournamentsQueryDto());

        // Act
        var result = await handler.Handle(query, default);

        // Assert
        result.Items.Count.Should().Be(0);
        result.Items.Should().BeEmpty();
    }
    

}
