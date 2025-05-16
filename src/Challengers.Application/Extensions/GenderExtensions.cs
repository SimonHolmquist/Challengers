using Challengers.Domain.Enums;

namespace Challengers.Application.Extensions;

public static class GenderExtensions
{
    public static string ToLocalizedString(this Gender gender)
    {
        return gender switch
        {
            Gender.Male => GetMessage(Gender_Male),
            Gender.Female => GetMessage(Gender_Female),
            _ => gender.ToString()
        };
    }
}
