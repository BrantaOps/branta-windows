using Branta.Classes;
using Branta.Classes.Wallets;
using Branta.Commands;
using Branta.Enums;
using Branta.Features.Main;
using Branta.Features.Settings;
using Branta.ViewModels;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace Branta.Features.WalletVerification;

public class VerifyWalletsCommand : BaseCommand
{
    private readonly NotificationCenter _notificationCenter;
    private readonly SettingsStore _settings;
    private readonly LanguageStore _languageStore;
    private readonly ILogger<VerifyWalletsCommand> _logger;

    public VerifyWalletsCommand(NotificationCenter notificationCenter, SettingsStore settings,
        LanguageStore languageStore, ILogger<VerifyWalletsCommand> logger)
    {
        _notificationCenter = notificationCenter;
        _settings = settings;
        _languageStore = languageStore;
        _logger = logger;
    }

    public override void Execute(object parameter)
    {
        var viewModel = (WalletVerificationViewModel)parameter;

        var previousWalletStatus = viewModel.Wallets
            .DistinctBy(w => w.Name)
            .ToDictionary(w => w.Name, w => w.Status);

        foreach (var walletType in viewModel.WalletTypes)
        {
            var (version, walletStatus) = Verify(walletType, _logger);

            var wallet = new Wallet
            {
                Name = walletType.Name,
                Version = version,
                LastScanned = DateTime.Now,
                Status = walletStatus,
                ExeName = walletType.ExeName
            };

            viewModel.AddWallet(wallet);

            if (!_settings.WalletVerification.WalletStatusChangeEnabled)
            {
                continue;
            }

            var previousStatus = previousWalletStatus.GetValueOrDefault(walletType.Name);

            if (previousStatus == null &&
                (wallet.Status == WalletStatus.Verified || wallet.Status == WalletStatus.NotFound))
            {
                continue;
            }

            if (previousStatus == wallet.Status)
            {
                continue;
            }

            _notificationCenter.Notify(new Notification
            {
                Message = $"{walletType.Name}: {wallet.Status.GetName(_languageStore)}",
                Icon = ToolTipIcon.None
            });
        }
    }

    public static (string, WalletStatus) Verify(BaseWalletType walletType, ILogger logger)
    {
        string version = null;
        try
        {
            if (walletType.InstallerRegex != null && Process.GetProcesses().Select(p => p.ProcessName)
                    .Any(walletType.InstallerRegex.IsMatch))
            {
                return (null, WalletStatus.Installing);
            }

            var path = walletType.GetPath();
            if (!Directory.Exists(path) || Directory.GetFiles(path).Length == 0)
            {
                return (null, WalletStatus.NotFound);
            }

            version = walletType.GetVersion(logger);

            if (version == null)
            {
                return (null, WalletStatus.VersionNotSupported);
            }

            var versionInfo = walletType.CheckSums.GetValueOrDefault(version);

            if (versionInfo == null)
            {
                var (newestVersion, oldestVersion) = walletType.GetNewestAndOldestSupportedVersion();

                var currentVersion = new Version(version);

                if (currentVersion > newestVersion)
                {
                    return (version, WalletStatus.VersionTooNew);
                }

                if (currentVersion < oldestVersion)
                {
                    return (version, WalletStatus.VersionTooOld);
                }

                return (version, WalletStatus.VersionNotSupported);
            }

            string hash = null;
            string expectedHash = null;

            if (versionInfo.Sha256 != null)
            {
                hash = CreateSha256ForFolder(path);
                expectedHash = versionInfo.Sha256;
            }

            if (hash == null)
            {
                return (version, WalletStatus.VersionNotSupported);
            }

            logger.LogInformation($"Wallet [{walletType.Name}] Expected: {expectedHash}; Actual: {hash}");

            return (version, hash == expectedHash ? WalletStatus.Verified : WalletStatus.NotVerified);
        }
        catch
        {
            return (version, WalletStatus.BrantaError);
        }
    }

    private static string CreateSha256ForFolder(string path)
    {
        var files = Directory
            .GetFiles(path, "*", SearchOption.AllDirectories)
            .OrderBy(p => p).ToList();

        var sha256 = SHA256.Create();

        for (var i = 0; i < files.Count; i++)
        {
            var file = files[i];

            var relativePath = file[(path.Length + 1)..];
            var pathBytes = Encoding.UTF8.GetBytes(relativePath.ToLower());
            sha256.TransformBlock(pathBytes, 0, pathBytes.Length, pathBytes, 0);

            var contentBytes = File.ReadAllBytes(file);
            if (i == files.Count - 1)
                sha256.TransformFinalBlock(contentBytes, 0, contentBytes.Length);
            else
                sha256.TransformBlock(contentBytes, 0, contentBytes.Length, contentBytes, 0);
        }

        return BitConverter.ToString(sha256.Hash ?? Array.Empty<byte>()).Replace("-", "").ToLower();
    }
}