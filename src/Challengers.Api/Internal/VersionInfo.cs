namespace Challengers.Api.Internal;

public static class VersionInfo
{
    public static string Version => typeof(Program).Assembly
        .GetName().Version?.ToString() ?? "unknown";

    public static string CommitSha => Environment.GetEnvironmentVariable("GIT_COMMIT_SHA") ?? "unknown";
}
