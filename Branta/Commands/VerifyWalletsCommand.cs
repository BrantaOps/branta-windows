using Branta.Classes;
using Branta.Classes.Wallets;
using Branta.Enums;
using Branta.Models;
using Branta.ViewModels;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using Microsoft.Extensions.Logging;

namespace Branta.Commands;

public class VerifyWalletsCommand : BaseCommand
{
    private readonly NotificationCenter _notificationCenter;
    private readonly Settings _settings;
    private readonly ILogger<VerifyWalletsCommand> _logger;

    public VerifyWalletsCommand(NotificationCenter notificationCenter, Settings settings, ILogger<VerifyWalletsCommand> logger)
    {
        _notificationCenter = notificationCenter;
        _settings = settings;
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
                Status = walletStatus
            };

            viewModel.AddWallet(wallet);

            if (wallet.Status != WalletStatus.Verified &&
                wallet.Status != WalletStatus.NotFound &&
                previousWalletStatus.GetValueOrDefault(walletType.Name, WalletStatus.Verified) == WalletStatus.Verified &&
                _settings.WalletVerification.WalletStatusChangeEnabled)
            {
                _notificationCenter.Notify(new Notification
                {
                    Message = $"{walletType.Name} failed verification.",
                    Icon = ToolTipIcon.Warning
                });
            }
        }
    }

    public static (string, WalletStatus) Verify(BaseWallet walletType, ILogger logger)
    {
        string version = null;
        try
        {
            if (walletType.InstallerRegex != null && Process.GetProcesses().Select(p => p.ProcessName).Any(walletType.InstallerRegex.IsMatch))
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
