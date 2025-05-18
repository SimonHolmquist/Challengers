using Challengers.Shared.Resources;

namespace Challengers.Shared.Helpers;

public static class ErrorMessages
{
    public static string InvalidGender()
    {
        return LocalizedMessages.FormatMessage(
            MessageKeys.InvalidGender,
            LocalizedMessages.GetMessage(MessageKeys.Gender_Male),
            LocalizedMessages.GetMessage(MessageKeys.Gender_Female)
        );
    }
}