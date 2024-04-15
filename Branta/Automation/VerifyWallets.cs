using Branta.Automation.Wallets;
using Branta.Classes;
using Branta.Domain;
using Branta.Enums;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace Branta.Automation;

public class VerifyWallets : BaseAutomation
{
    public ObservableCollection<Wallet> Wallets { get; } = new();
    public static List<BaseWallet> WalletTypes = new();

    private readonly BrantaClient _brantaClient;

    private bool _isFirstRun = true;

    public VerifyWallets(NotifyIcon notifyIcon, Settings settings) : base(notifyIcon, settings,
        (int)settings.WalletVerification.WalletVerifyEvery.TotalSeconds)
    {
        _brantaClient = new BrantaClient();

        Task.Run(LoadCheckSums);
    }

    public override void Run()
    {
        Trace.WriteLine("Started: Verify Wallets");
        var sw = Stopwatch.StartNew();

        var previousWalletStatus = Wallets
            .DistinctBy(w => w.Name)
            .ToDictionary(w => w.Name, w => w.Status);

        var bufferedWallets = new List<Wallet>();

        foreach (var walletType in WalletTypes)
        {
            var wallet = Verify(walletType);

            if (wallet == null)
            {
                continue;
            }

            if (wallet.Status != WalletStatus.Verified &&
                previousWalletStatus.GetValueOrDefault(walletType.Name, WalletStatus.Verified) ==
                WalletStatus.Verified &&
                Settings.WalletVerification.WalletStatusChangeEnabled)
            {
                NotifyIcon.ShowBalloonTip(new Notification
                {
                    Message = $"{walletType.Name} failed verification.",
                    Icon = ToolTipIcon.Warning
                });
            }

            if (_isFirstRun)
            {
                Dispatcher.Invoke(() => Wallets.Add(wallet));
            }
            else
            {
                bufferedWallets.Add(wallet);
            }
        }

        if (!_isFirstRun)
        {
            Dispatcher.Invoke(() => Wallets.Set(bufferedWallets));
        }

        _isFirstRun = false;

        sw.Stop();
        Trace.WriteLine($"Stopped: Verify Wallets. Took {sw.Elapsed}");
    }

    public static Wallet Verify(BaseWallet walletType)
    {
        var path = walletType.GetPath();
        if (!Directory.Exists(path) || Directory.GetFiles(path).Length == 0)
        {
            return null;
        }

        var version = walletType.GetVersion();

        var versionInfo = version != null ? walletType.CheckSums.GetValueOrDefault(version) : null;

        if (versionInfo == null)
        {
            return new Wallet
            {
                Name = walletType.Name,
                Version = version,
                Status = WalletStatus.VersionNotSupported
            };
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
            else if (versionInfo.Md5 != null)
            {
                hash = CreateMd5ForFolder(path);
                expectedHash = versionInfo.Md5;
            }

            if (hash == null)
            {
                return new Wallet
                {
                    Name = walletType.Name,
                    Version = version,
                    Status = WalletStatus.VersionNotSupported
                };
            }

            Trace.WriteLine($"Wallet [{walletType.Name}] Expected: {expectedHash}; Actual: {hash}");
            return new Wallet
            {
                Name = walletType.Name,
                Version = version,
                Status = hash == expectedHash ? WalletStatus.Verified : WalletStatus.NotVerified
            };
        }
        catch
        {
            return new Wallet
            {
                Name = walletType.Name,
                Version = version,
                Status = WalletStatus.NotVerified
            };
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

            var relativePath = file.Substring(path.Length + 1);
            var pathBytes = Encoding.UTF8.GetBytes(relativePath.ToLower());
            sha256.TransformBlock(pathBytes, 0, pathBytes.Length, pathBytes, 0);

            var contentBytes = File.ReadAllBytes(file);
            if (i == files.Count - 1)
                sha256.TransformFinalBlock(contentBytes, 0, contentBytes.Length);
            else
                sha256.TransformBlock(contentBytes, 0, contentBytes.Length, contentBytes, 0);
        }

        return BitConverter.ToString(sha256.Hash).Replace("-", "").ToLower();
    }

    private static string CreateMd5ForFolder(string path)
    {
        var files = Directory
            .GetFiles(path, "*", SearchOption.AllDirectories)
            .OrderBy(p => p).ToList();

        var md5 = MD5.Create();

        for (var i = 0; i < files.Count; i++)
        {
            var file = files[i];

            var relativePath = file.Substring(path.Length + 1);
            var pathBytes = Encoding.UTF8.GetBytes(relativePath.ToLower());
            md5.TransformBlock(pathBytes, 0, pathBytes.Length, pathBytes, 0);

            var contentBytes = File.ReadAllBytes(file);
            if (i == files.Count - 1)
                md5.TransformFinalBlock(contentBytes, 0, contentBytes.Length);
            else
                md5.TransformBlock(contentBytes, 0, contentBytes.Length, contentBytes, 0);
        }

        return BitConverter.ToString(md5.Hash).Replace("-", "").ToLower();
    }

    private async Task LoadCheckSums()
    {
        var checkSums = await _brantaClient.GetCheckSumsAsync() ?? YamlLoader.LoadCheckSums();

        WalletTypes = new List<BaseWallet>
        {
            new BlockStreamGreen
            {
                CheckSums = checkSums.BlockstreamGreen
            },
            new Ledger
            {
                CheckSums = checkSums.Ledger
            },
            new Sparrow
            {
                CheckSums = checkSums.Sparrow
            },
            new Trezor
            {
                CheckSums = checkSums.Trezor
            },
            new Wasabi
            {
                CheckSums = checkSums.Wasabi
            }
        };
    }
}