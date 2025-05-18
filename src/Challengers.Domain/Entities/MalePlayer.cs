using Challengers.Domain.Enums;
using System.Globalization;

namespace Challengers.Domain.Entities;

public class MalePlayer : Player
{
    public int Strength { get; private set; }
    public int Speed { get; private set; }

    public MalePlayer(string name, string surname, int skill, int strength, int speed)
        : base(name, surname, skill, Gender.Male)
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

    protected override double CalculateScoreWithLuck(double luck)
    {
        return Skill * MaleSkillWeight +
               Strength * MaleStrengthWeight +
               Speed * MaleSpeedWeight +
               luck * LuckWeight;
    }

    public override string ExplainScore(double score, double luck)
    {
        return FormatMessage(
            MaleScoreExplanation,
            Skill, MaleSkillWeight,
            Strength, MaleStrengthWeight,
            Speed, MaleSpeedWeight,
            luck.ToString(CultureInfo.InvariantCulture), LuckWeight,
            score
        );
    }
    public void SetStrength(int value) => Strength = value;
    public void SetSpeed(int value) => Speed = value;

}
