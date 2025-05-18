namespace Challengers.Domain.Constants;

public static class PlayerConstants
{
    public const int MaxNameLength = 50;
    public const int MaxSurnameLength = 50;
    public const int MinStat = 0;
    public const int MaxStat = 100;

    public const double MaleSkillWeight = 0.4;
    public const double MaleStrengthWeight = 0.3;
    public const double MaleSpeedWeight = 0.1;

    public const double FemaleSkillWeight = 0.3;
    public const double FemaleReactionTimeWeight = 0.3;
    public const double LuckWeight = 100;

    public const int DefaultPage = 1;
    public const int DefaultPageSize = 20;
}
