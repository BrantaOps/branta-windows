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

namespace Branta.Commands;

public class VerifyWalletsCommand : BaseCommand
{
    private readonly WalletVerificationViewModel _viewModel;
    private readonly NotificationCenter _notificationCenter;
    private readonly Settings _settings;

    public VerifyWalletsCommand(WalletVerificationViewModel viewModel, NotificationCenter notificationCenter, Settings settings)
    {
        _viewModel = viewModel;
        _notificationCenter = notificationCenter;
        _settings = settings;
    }

    public override void Execute(object parameter)
    {
        var previousWalletStatus = _viewModel.Wallets
            .DistinctBy(w => w.Name)
            .ToDictionary(w => w.Name, w => w.Status);

        foreach (var walletType in _viewModel.WalletTypes)
        {
            var (version, walletStatus) = Verify(walletType);

            var wallet = new Wallet()
            {
                Name = walletType.Name,
                Version = version,
                LastScanned = DateTime.Now,
                Status = walletStatus
            };

            _viewModel.AddWallet(wallet);

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

    public static (string, WalletStatus) Verify(BaseWallet walletType)
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

        var version = walletType.GetVersion();

        var versionInfo = version != null ? walletType.CheckSums.GetValueOrDefault(version) : null;

        if (versionInfo == null)
        {
            return (version, WalletStatus.VersionNotSupported);
        }

        try
        {
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

            Trace.WriteLine($"Wallet [{walletType.Name}] Expected: {expectedHash}; Actual: {hash}");

            return (version, hash == expectedHash ? WalletStatus.Verified : WalletStatus.NotVerified);
        }
        catch
        {
            return (version, WalletStatus.NotVerified);
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
