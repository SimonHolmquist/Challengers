using Challengers.Domain.Services;
using Moq;

namespace Challengers.UnitTests.Helpers;

public static class MoqExtensions
{
    public static void SetupRepeatedReturns(this Mock<IRandomGenerator> mock, int count, double value = 0.5)
    {
        var sequence = mock.SetupSequence(r => r.NextDouble());
        for (int i = 0; i < count; i++)
        {
            sequence = sequence.Returns(value);
        }
    }
    public static void SetupReturnsFromList(this Mock<IRandomGenerator> mock, IEnumerable<double> values)
    {
        var sequence = mock.SetupSequence(r => r.NextDouble());
        foreach (var value in values)
        {
            sequence = sequence.Returns(value);
        }
    }
}
