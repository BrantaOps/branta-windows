using System.IO;
using System.Reflection;
using System.Security.Cryptography;

namespace Branta.Classes;

public static class Helper
{
    public static string GetBrantaVersionWithoutCommitHash()
    {
        var version = Assembly.GetEntryAssembly()!.GetCustomAttribute<AssemblyInformationalVersionAttribute>()!
            .InformationalVersion;

        return version.Split("+")[0];
    }

    public static string CalculateSha256(string filePath)
    {
        using var sha256 = SHA256.Create();
        using var stream = File.OpenRead(filePath);
        var hashBytes = sha256.ComputeHash(stream);
        return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
    }
}