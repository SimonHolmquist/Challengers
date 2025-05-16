using Challengers.Shared.Resources;
using System.Globalization;
using System.Resources;

namespace Challengers.Shared.Helpers;

public static class LocalizedMessages
{
    private static readonly ResourceManager _resourceManager = new(typeof(Messages));
    private const string FallbackCulture = "en";

    public static string GetMessage(string key)
    {
        var message = _resourceManager.GetString(key, CultureInfo.CurrentUICulture);
        if (!string.IsNullOrEmpty(message))
            return message;

        message = _resourceManager.GetString(key, new CultureInfo(FallbackCulture));
        return message ?? key;
    }

    public static string FormatMessage(string key, params object[] args)
    {
        var template = GetMessage(key);
        try
        {
            return string.Format(template, args);
        }
        catch (FormatException)
        {
            return string.Format(GetMessage(MessageKeys.FormatError), template);
        }
    }
}
