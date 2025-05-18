namespace Challengers.Domain.Services;

public class DefaultRandomGenerator : IRandomGenerator
{
    private readonly Random _random = new();

    public double NextDouble() => _random.NextDouble();
}
