namespace Challengers.Domain.Entities;

public class Match
{
    public Player Player1 { get; }
    public Player Player2 { get; }
    public Player? Winner { get; private set; }

    private bool _isSimulated;

    public Match(Player player1, Player player2)
    {
        Player1 = player1 ?? throw new ArgumentException(MatchPlayersRequired);
        Player2 = player2 ?? throw new ArgumentException(MatchPlayersRequired);

        if (player1.Id == player2.Id)
            throw new ArgumentException(MatchSamePlayer);
    }

    public void Simulate(Random? rng = null)
    {
        if (_isSimulated)
            throw new InvalidOperationException(MatchAlreadySimulated);

        rng ??= new Random();

        var luck1 = Player1.GenerateLuck(rng);
        var luck2 = Player2.GenerateLuck(rng);

        var score1 = Player1.GetMatchScore(luck1);
        var score2 = Player2.GetMatchScore(luck2);


        Winner = score1 > score2 ? Player1 :
                 score2 > score1 ? Player2 :
                 Player1.Id.CompareTo(Player2.Id) < 0 ? Player1 : Player2;

        _isSimulated = true;
    }
}