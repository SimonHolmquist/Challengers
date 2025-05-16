using Challengers.Domain.Enums;

namespace Challengers.Domain.Entities;

public class Tournament
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public string Name { get; private set; }
    public Gender Gender { get; private set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    private readonly List<Match> _matches = [];
    public IReadOnlyCollection<Match> Matches => _matches.AsReadOnly();

    public Player? Winner { get; private set; }

    private bool _isCompleted = false;

    public Tournament(string name, Gender gender)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(GetMessage(TournamentNameRequired), nameof(name));

        if (gender != Gender.Male && gender != Gender.Female)
            throw new ArgumentException(GetMessage(TournamentGenderInvalid));

        Name = name;
        Gender = gender;
    }

    public void Simulate(IList<Player> players, Random? rng = null)
    {
        if (_isCompleted)
            throw new InvalidOperationException(GetMessage(TournamentAlreadyCompleted));

        if (players == null || players.Count < MinPlayers || !IsPowerOfTwo(players.Count))
            throw new ArgumentException(TournamentInvalidPlayerCount);

        rng ??= new Random();

        var currentPlayers = new List<Player>(players);
        while (currentPlayers.Count >= MinPlayers)
        {
            var matches = new List<Match>();

            for (int i = 0; i < currentPlayers.Count; i += MinPlayers)
            {
                var match = new Match(currentPlayers[i], currentPlayers[i + 1]);
                match.Simulate(rng);
                matches.Add(match);
            }

            _matches.AddRange(matches);
            currentPlayers = [.. matches.Select(m => m.Winner!)];
        }

        Winner = currentPlayers.Single();
        _isCompleted = true;
    }

    private static bool IsPowerOfTwo(int value) => value > 0 && (value & (value - 1)) == 0;
}
