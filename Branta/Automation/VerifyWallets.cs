using Branta.Automation.Wallets;
using Branta.Domain;
using Branta.Enums;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using Branta.Classes;

namespace Branta.Automation;

public class VerifyWallets : BaseAutomation
{
    public ObservableCollection<Wallet> Wallets { get; } = new();
    private List<Wallet> BufferedWallets { get; set; }

    private readonly List<BaseWallet> _walletTypes;

    public VerifyWallets(NotifyIcon notifyIcon) : base(notifyIcon, 10)
    {
        _walletTypes = new List<BaseWallet>
        {
            new Sparrow(),
            new Wasabi()
        }.OrderBy(w => w.Name).ToList();
    }

    public override void Run()
    {
        Trace.WriteLine("Started: Verify Wallets");
        var sw = Stopwatch.StartNew();

        BufferedWallets = new List<Wallet>();

        var previousWalletStatus = Wallets
            .DistinctBy(w => w.Name)
            .ToDictionary(w => w.Name, w => w.Status);

        foreach (var walletType in _walletTypes)
        {
            if (!Directory.Exists(walletType.GetPath()))
            {
                continue;
            }

            var version = walletType.GetVersion();

            WalletStatus status;

            var expectedHash = version != null ? walletType.CheckSums.GetValueOrDefault(version) : null;

            if (expectedHash == null)
            {
                status = WalletStatus.VersionNotSupported;
            }
            else
            {
                try
                {
                    var hash = CreateMd5ForFolder(walletType.GetPath());
                    Trace.WriteLine($"Expected: {expectedHash}; Actual: {hash}");
                    status = hash == expectedHash ? WalletStatus.Verified : WalletStatus.NotVerified;
                }
                catch
                {
                    status = WalletStatus.NotVerified;
                }
            }

            if (status != WalletStatus.Verified && previousWalletStatus.GetValueOrDefault(walletType.Name, WalletStatus.Verified) == WalletStatus.Verified)
            {
                NotifyIcon.ShowBalloonTip(new Notification
                {
                    Message = $"{walletType.Name} failed verification.",
                    Icon = ToolTipIcon.Warning
                });
            }

            BufferedWallets.Add(new Wallet
            {
                Name = walletType.Name,
                Version = version,
                Status = status
            });
        }

        sw.Stop();
        Trace.WriteLine($"Stopped: Verify Wallets. Took {sw.Elapsed}");
    }

    public override void Update()
    {
        Wallets.Clear();

        foreach (var wallet in BufferedWallets)
        {
            Wallets.Add(wallet);
        }
    }

    public static string CreateMd5ForFolder(string path)
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
}
