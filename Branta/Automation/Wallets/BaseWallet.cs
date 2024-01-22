using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace Branta.Automation.Wallets;

public abstract partial class BaseWallet
{
    public string Name { get; }

    private readonly string _exeName;

    public abstract IReadOnlyDictionary<string, string> CheckSums { get; }

    protected BaseWallet(string name, string exeName = null)
    {
        Name = name;
        _exeName = exeName ?? name;
    }

    public abstract string GetPath();

    public string GetVersion()
    {
        var fileVersionInfo = FileVersionInfo.GetVersionInfo(Path.Join(GetPath(), $"{_exeName}.exe"));
        if (fileVersionInfo.FileVersion == null)
        {
            return null;
        }

        var match = VersionRegex().Match(fileVersionInfo.FileVersion);

        return match.Success ? match.Groups[1].Value : null;
    }

    [GeneratedRegex(@"(\d+\.\d+\.\d+)")]
    private static partial Regex VersionRegex();
}
