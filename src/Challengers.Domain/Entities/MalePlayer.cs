using Challengers.Domain.Enums;

namespace Challengers.Domain.Entities;

public class MalePlayer : Player
{
    public int Strength { get; private set; }
    public int Speed { get; private set; }

    public MalePlayer(string name, int skill, int strength, int speed)
        : base(name, skill, Gender.Male)
    {
        if (strength is < MinStat or > MaxStat)
            throw new ArgumentOutOfRangeException(
                nameof(strength),
                FormatMessage(StrengthOutOfRange, MinStat, MaxStat));

        if (speed is < MinStat or > MaxStat)
            throw new ArgumentOutOfRangeException(
                nameof(speed),
                FormatMessage(SpeedOutOfRange, MinStat, MaxStat));

        Strength = strength;
        Speed = speed;
    }

    protected override double CalculateScoreWithLuck(int luck)
    {
        return Skill * MaleSkillWeight +
               Strength * MaleStrengthWeight +
               Speed * MaleSpeedWeight +
               luck * LuckWeight;
    }

    public override string ExplainScore(double score, int luck)
    {
        return FormatMessage(
            MaleScoreExplanation,
            Skill, MaleSkillWeight,
            Strength, MaleStrengthWeight,
            Speed, MaleSpeedWeight,
            luck, LuckWeight,
            score
        );
    }
}
