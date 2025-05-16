using Challengers.Domain.Enums;

namespace Challengers.Domain.Entities;

public class Tournament
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public string Name { get; private set; }
    public Gender Gender { get; private set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    private readonly List<Player> _players = [];
    public IReadOnlyCollection<Player> Players => _players.AsReadOnly();

    private readonly List<Match> _matches = [];
    public IReadOnlyCollection<Match> Matches => _matches.AsReadOnly();

    public Player? Winner { get; private set; }

    private bool _isCompleted = false;

    public Tournament(string name, Gender gender, IList<Player> players)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(GetMessage(TournamentNameRequired), nameof(name));

        if (gender != Gender.Male && gender != Gender.Female)
            throw new ArgumentException(GetMessage(TournamentGenderInvalid));

        if (players is null || players.Count < MinPlayers || !IsPowerOfTwo(players.Count))
            throw new ArgumentException(GetMessage(TournamentInvalidPlayerCount), nameof(players));

        Name = name;
        Gender = gender;
        _players.AddRange(players);

    }

    public void Simulate(Random? rng = null)
    {
        if (_isCompleted)
            throw new InvalidOperationException(GetMessage(TournamentAlreadyCompleted));

        if (_players.Count < MinPlayers || !IsPowerOfTwo(_players.Count))
            throw new ArgumentException(GetMessage(TournamentInvalidPlayerCount));

        rng ??= new Random();
        var queue = new Queue<Player>(_players);

        while (queue.Count > 1)
        {
            var match = new Match(queue.Dequeue(), queue.Dequeue());
            match.Simulate(rng);
            _matches.Add(match);
            queue.Enqueue(match.Winner!);
        }

        Winner = queue.Dequeue();
        _isCompleted = true;
    }

    private static bool IsPowerOfTwo(int value) => value > 0 && (value & (value - 1)) == 0;
}
