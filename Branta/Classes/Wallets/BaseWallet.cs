using Branta.Enums;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace Branta.Classes.Wallets;

public abstract partial class BaseWallet(string name, string exeName = null)
{
    public string Name { get; } = name;

    public string ExeName { get; } = exeName ?? name;

    public string InstallerName { get; set; }

    public HashType InstallerHashType { get; set; } = HashType.Sha256;

    public Dictionary<string, VersionInfo> CheckSums { get; set; }

    public Regex InstallerRegex { get; set; }

    private const string RegistryUninstallPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\";

    public abstract string GetPath();

    public string GetVersion(ILogger logger)
    {
        var fileVersionInfo = FileVersionInfo.GetVersionInfo(Path.Join(GetPath(), $"{ExeName}.exe"));

        if (fileVersionInfo.FileVersion != null)
        {
            var match = VersionRegex().Match(fileVersionInfo.FileVersion);
            return match.Success ? match.Groups[1].Value : null;
        }

        var subKeyName = RegistryHelper.FindDisplayName(RegistryUninstallPath, Name, logger)
            .Replace("HKEY_LOCAL_MACHINE\\", "");
        logger.LogInformation(subKeyName);

        return RegistryHelper.GetValue(subKeyName, "DisplayVersion", logger);
    }

    public (Version, Version) GetNewestAndOldestSupportedVersion()
    {
        var versions = CheckSums
            .Select(c => new Version(c.Key))
            .OrderByDescending(v => v)
            .ToList();

        return (versions.First(), versions.Last());
    }

    [GeneratedRegex(@"(\d+\.\d+\.\d+)")]
    private static partial Regex VersionRegex();

    public static List<BaseWallet> GetWalletTypes(CheckSums checkSums = null)
    {
        return new List<BaseWallet>
        {
            // new BlockStreamGreen
            // {
            //     CheckSums = checkSums?.BlockstreamGreen
            // },
            // new Ledger
            // {
            //     CheckSums = checkSums?.Ledger
            // },
            new Sparrow
            {
                CheckSums = checkSums?.Sparrow
            },
            // new Trezor
            // {
            //     CheckSums = checkSums?.Trezor
            // },
            // new Wasabi
            // {
            //     CheckSums = checkSums?.Wasabi
            // }
        };
    }
}