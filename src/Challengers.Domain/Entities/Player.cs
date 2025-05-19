using Challengers.Domain.Common;
using Challengers.Domain.Enums;
using Challengers.Domain.Services;

namespace Challengers.Domain.Entities;

public abstract class Player : Entity<Guid>
{
    public string FirstName { get; private set; } = default!;
    public string LastName { get; private set; } = default!;
    public string FullName { get => $"{FirstName} {LastName}"; }
    public int Skill { get; private set; }
    public Gender Gender { get; private set; }
    public List<Tournament> Tournaments { get; private set; } = [];
    protected Player(string firstName, string lastName, int skill, Gender gender)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException(GetMessage(FirstNameRequired));

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException(GetMessage(LastNameRequired));

        if (skill is < MinStat or > MaxStat)
            throw new ArgumentOutOfRangeException(nameof(skill),
                FormatMessage(SkillOutOfRange, MinStat, MaxStat));

        FirstName = firstName;
        LastName = lastName;
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
    public void SetName(string name) => FirstName = name;
    public void SetLastName(string lastname) => LastName = lastname;
    public void SetSkill(int skill) => Skill = skill;
}