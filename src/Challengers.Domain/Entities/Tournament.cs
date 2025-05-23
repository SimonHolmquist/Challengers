﻿using Challengers.Domain.Common;
using Challengers.Domain.Enums;
using Challengers.Domain.Services;
using Challengers.Shared.Helpers;
using System.ComponentModel.DataAnnotations.Schema;

namespace Challengers.Domain.Entities;
public class Tournament : Entity<Guid>
{
    public string Name { get; private set; } = default!;
    public Gender Gender { get; private set; }
    public List<Player> Players { get; private set; } = [];
    public List<Match> Matches { get; private set; } = [];

    public Guid? WinnerId { get; private set; }
    public Player? Winner { get; private set; }

    [Column("IsCompleted")]
    public bool IsCompleted
    {
        get => _isCompleted;
        private set => _isCompleted = value;
    }
    private bool _isCompleted = false;

    private Tournament() { }
    public Tournament(string name, Gender gender, IList<Player> players)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(GetMessage(TournamentNameRequired));

        if (gender != Gender.Male && gender != Gender.Female)
            throw new ArgumentException(ErrorMessages.InvalidGender());

        if (players is null || players.Count < MinPlayers || !IsPowerOfTwo(players.Count))
            throw new ArgumentException(GetMessage(TournamentInvalidPlayerCount));

        Name = name;
        Gender = gender;
        Players.AddRange(players);
    }

    public void Simulate(IRandomGenerator? rng = null)
    {
        if (_isCompleted)
            throw new InvalidOperationException(GetMessage(TournamentAlreadyCompleted));

        if (Players.Count < MinPlayers || !IsPowerOfTwo(Players.Count))
            throw new ArgumentException(GetMessage(TournamentInvalidPlayerCount));

        rng ??= new DefaultRandomGenerator();
        var queue = new Queue<Player>(Players);

        while (queue.Count > 1)
        {
            var match = new Match(queue.Dequeue(), queue.Dequeue(), rng);
            match.SetTournament(this);
            match.Simulate();
            Matches.Add(match);
            queue.Enqueue(match.Winner!);
        }

        Winner = queue.Dequeue();
        WinnerId = Winner.Id;
        _isCompleted = !_isCompleted;
    }

    private static bool IsPowerOfTwo(int value) => value > 0 && (value & (value - 1)) == 0;
}
