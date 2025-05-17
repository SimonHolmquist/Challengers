using Challengers.Domain.Enums;

namespace Challengers.Domain.Entities;

public abstract class Player
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Name { get; private set; }
    public int Skill { get; private set; }
    public Gender Gender { get; private set; }
    public List<Tournament> Tournaments { get; private set; } = [];
    protected Player(string name, int skill, Gender gender)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(NameRequired, nameof(name));

        if (skill is < MinStat or > MaxStat)
            throw new ArgumentOutOfRangeException(nameof(skill),
                FormatMessage(SkillOutOfRange, MinStat, MaxStat));

        Name = name;
        Skill = skill;
        Gender = gender;
    }

    public static int GenerateLuck(Random? rng)
    {
        rng ??= new Random();
        return rng.Next(MinLuck, MaxLuck + LuckRangeAdjustment);
    }

    public double GetMatchScore(int luck)
    {
        return CalculateScoreWithLuck(luck);
    }

    protected abstract double CalculateScoreWithLuck(int luck);
    public abstract string ExplainScore(double score, int luck);
    public void SetName(string name) => Name = name;
    public void SetSkill(int skill) => Skill = skill;
}