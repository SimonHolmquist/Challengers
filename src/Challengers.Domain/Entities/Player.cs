using Challengers.Domain.Common;
using Challengers.Domain.Enums;
using Challengers.Domain.Services;

namespace Challengers.Domain.Entities;

public abstract class Player : Entity<Guid>
{
    public string Name { get; private set; } = default!;
    public string Surname { get; private set; } = default!;
    public int Skill { get; private set; }
    public Gender Gender { get; private set; }
    public List<Tournament> Tournaments { get; private set; } = [];
    protected Player(string name, string surname, int skill, Gender gender)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(NameRequired, nameof(name));

        if (string.IsNullOrWhiteSpace(surname))
            throw new ArgumentException(SurnameRequired, nameof(surname));

        if (skill is < MinStat or > MaxStat)
            throw new ArgumentOutOfRangeException(nameof(skill),
                FormatMessage(SkillOutOfRange, MinStat, MaxStat));

        Name = name;
        Surname = surname;
        Skill = skill;
        Gender = gender;
    }

    public static double GenerateLuck(IRandomGenerator rng)
    {
        return rng.NextDouble();
    }

    public double GetMatchScore(double luck)
    {
        return CalculateScoreWithLuck(luck);
    }

    protected abstract double CalculateScoreWithLuck(double luck);
    public abstract string ExplainScore(double score, double luck);
    public void SetName(string name) => Name = name;
    public void SetSurname(string surname) => Surname = surname;
    public void SetSkill(int skill) => Skill = skill;
    public string GetFullName() => $"{Name} {Surname}";
}