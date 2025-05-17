namespace Challengers.Domain.Entities;

public class Match
{
    public Guid Id { get; private set; }

    public Guid TournamentId { get; private set; }
    public Tournament Tournament { get; private set; } = null!;

    public Guid Player1Id { get; private set; }
    public Player Player1 { get; private set; } = default!;

    public Guid Player2Id { get; private set; }
    public Player Player2 { get; private set; } = default!;

    public Guid? WinnerId { get; private set; }
    public Player? Winner { get; private set; }

    private bool _isSimulated;

    private Match() { }

    public Match(Player player1, Player player2)
    {
        Player1 = player1 ?? throw new ArgumentException(MatchPlayersRequired);
        Player2 = player2 ?? throw new ArgumentException(MatchPlayersRequired);

        Player1Id = player1.Id;
        Player2Id = player2.Id;

        if (player1.Id == player2.Id)
            throw new ArgumentException(MatchSamePlayer);
    }

    public void SetTournament(Tournament tournament)
    {
        Tournament = tournament ?? throw new ArgumentNullException(nameof(tournament));
        TournamentId = tournament.Id;
    }

    public void Simulate(Random? rng = null)
    {
        if (_isSimulated)
            throw new InvalidOperationException(MatchAlreadySimulated);

        rng ??= new Random();

        var luck1 = Player.GenerateLuck(rng);
        var luck2 = Player.GenerateLuck(rng);

        var score1 = Player1.GetMatchScore(luck1);
        var score2 = Player2.GetMatchScore(luck2);

        Winner = score1 > score2 ? Player1 :
                 score2 > score1 ? Player2 :
                 Player1.Id.CompareTo(Player2.Id) < 0 ? Player1 : Player2;

        WinnerId = Winner.Id;
        _isSimulated = true;
    }
}
