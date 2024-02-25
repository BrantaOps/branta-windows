using System.Reflection;

namespace Branta.Classes;

public static class Helper
{
    public static string GetBrantaVersionWithoutCommitHash()
    {
        var version = Assembly.GetEntryAssembly()!.GetCustomAttribute<AssemblyInformationalVersionAttribute>()!.InformationalVersion;

        return version.Split("+")[0];
    }
}
