using Branta.Classes;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace Branta.Automation.Wallets;

public abstract partial class BaseWallet
{
    public string Name { get; }

    public string ExeName { get; }

    public abstract IReadOnlyDictionary<string, string> CheckSums { get; }

    public abstract IReadOnlyDictionary<string, string> InstallerHashes { get; }

    private const string RegistryUninstallPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\";

    protected BaseWallet(string name, string exeName = null)
    {
        Name = name;
        ExeName = exeName ?? name;
    }

    public abstract string GetPath();

    public string GetVersion()
    {
        var fileVersionInfo = FileVersionInfo.GetVersionInfo(Path.Join(GetPath(), $"{ExeName}.exe"));

        if (fileVersionInfo.FileVersion != null)
        {
            var match = VersionRegex().Match(fileVersionInfo.FileVersion);
            return match.Success ? match.Groups[1].Value : null;
        }

        var subKeyName = RegistryHelper.FindDisplayName(RegistryUninstallPath, Name)
            .Replace("HKEY_LOCAL_MACHINE\\", "");
        Trace.WriteLine(subKeyName);

        if (subKeyName != null)
        {
            return RegistryHelper.GetValue(subKeyName, "DisplayVersion");
        }

        return null;
    }

    [GeneratedRegex(@"(\d+\.\d+\.\d+)")]
    private static partial Regex VersionRegex();
}
