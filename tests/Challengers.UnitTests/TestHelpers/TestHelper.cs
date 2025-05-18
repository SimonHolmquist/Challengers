using Challengers.Domain.Common;
using Challengers.Domain.Entities;

namespace Challengers.UnitTests.TestHelpers;

public static class TestHelper
{
    public static T WithId<T>(this T entity, Guid id) where T : Entity<Guid>
    {
        typeof(T).GetProperty(nameof(Entity<Guid>.Id))!
            .SetValue(entity, id);
        return entity;
    }

    public static T WithRandomId<T>(this T entity) where T : Entity<Guid> => entity.WithId(Guid.NewGuid());

    public static List<Player> FakePlayers(bool Male, int count)
    {
        return [.. Enumerable.Range(1, count).Select<int, Player>(i =>
            Male
                ? new MalePlayer($"P{i}", $"M{i}", 80, 70, 60).WithRandomId()
                : new FemalePlayer($"P{i}", $"F{i}", 80, 75).WithRandomId()
        )];
    }

    public static Tournament SimulateAndReturn(this Tournament t)
    {
        t.Simulate(); return t;
    }
}
