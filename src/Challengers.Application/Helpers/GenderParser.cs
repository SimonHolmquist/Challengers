using Challengers.Domain.Enums;
using Challengers.Shared.Helpers;
using Challengers.Shared.Resources;
using System.Globalization;

namespace Challengers.Application.Helpers;

public static class GenderParser
{
    public static bool TryParse(string input, out Gender gender)
    {
        input = input.Trim().ToLowerInvariant();
        var male = GetMessage(Gender_Male).ToLowerInvariant();
        var female = GetMessage(Gender_Female).ToLowerInvariant();

        if (input == male) { gender = Gender.Male; return true; }
        if (input == female) { gender = Gender.Female; return true; }

        gender = default;
        return false;
    }
}