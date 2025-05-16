using Challengers.Domain.Resources;
using System.Globalization;
using System.Resources;

namespace Challengers.Domain.Helpers;

public static class LocalizedMessages
{
    private static readonly ResourceManager _resourceManager =
        new(typeof(Messages));

    public static string GetMessage(string key)
    {
        return _resourceManager.GetString(key, CultureInfo.CurrentUICulture)
               ?? $"[{key}]";
    }

    public static string FormatMessage(string key, params object[] args)
    {
        var template = GetMessage(key);
        return string.Format(template, args);
    }
}