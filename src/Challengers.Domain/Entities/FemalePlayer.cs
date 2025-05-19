using Challengers.Domain.Enums;
using System.Globalization;

namespace Challengers.Domain.Entities;

public class FemalePlayer : Player
{
    public int ReactionTime { get; private set; }

    public FemalePlayer(string firstName, string lastName, int skill, int reactionTime)
        : base(firstName, lastName, skill, Gender.Female)
    {
        if (reactionTime is < MinStat or > MaxStat)
            throw new ArgumentOutOfRangeException(
                nameof(reactionTime),
                    FormatMessage(ReactionOutOfRange, MinStat, MaxStat));

        ReactionTime = reactionTime;
    }

    protected override double CalculateScoreWithLuck(double luck)
    {
        return Skill * FemaleSkillWeight +
               ReactionTime * FemaleReactionTimeWeight +
               luck * LuckWeight;
    }

    public override string ExplainScore(double score, double luck)
    {
        return FormatMessage(
            FemaleScoreExplanation,
            Skill, FemaleSkillWeight,
            ReactionTime, FemaleReactionTimeWeight,
            luck.ToString(CultureInfo.InvariantCulture), LuckWeight,
            score
        );
    }
    public void SetReactionTime(int value) => ReactionTime = value;

}
