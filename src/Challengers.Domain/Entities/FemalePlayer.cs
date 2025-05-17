using Challengers.Domain.Enums;

namespace Challengers.Domain.Entities;

public class FemalePlayer : Player
{
    public int ReactionTime { get; private set; }

    public FemalePlayer(string name, int skill, int reactionTime)
        : base(name, skill, Gender.Female)
    {
        if (reactionTime is < MinStat or > MaxStat)
            throw new ArgumentOutOfRangeException(
                nameof(reactionTime),
                    FormatMessage(ReactionOutOfRange, MinStat, MaxStat));

        ReactionTime = reactionTime;
    }

    protected override double CalculateScoreWithLuck(int luck)
    {
        return Skill * FemaleSkillWeight +
               ReactionTime * FemaleReactionTimeWeight +
               luck * LuckWeight;
    }

    public override string ExplainScore(double score, int luck)
    {
        return FormatMessage(
            FemaleScoreExplanation,
            Skill, FemaleSkillWeight,
            ReactionTime, FemaleReactionTimeWeight,
            luck, LuckWeight,
            score
        );
    }
    public void SetReactionTime(int value) => ReactionTime = value;

}
