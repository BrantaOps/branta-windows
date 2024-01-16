using Branta.Automation.Wallets;
using Branta.Domain;
using Branta.Enums;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace Branta.Automation;

public class VerifyWallet
{
    public static List<Wallet> Run(Dictionary<string, WalletStatus> previousWallets, NotifyIcon notifyIcon)
    {
        var wallets = new List<Wallet>();

        var walletTypes = new List<BaseWallet>
        {
            new Sparrow(),
            new Wasabi()
        };

        foreach (var wallet in walletTypes)
        {
            if (!Directory.Exists(wallet.GetPath()))
            {
                continue;
            }

            var version = wallet.GetVersion();

            WalletStatus status;

            var expectedHash = version != null ? wallet.CheckSums.GetValueOrDefault(version) : null;

            if (expectedHash == null)
            {
                status = WalletStatus.VersionNotSupported;
            }
            else
            {
                var hash = CreateMd5ForFolder(wallet.GetPath());
                Trace.WriteLine($"Expected: {expectedHash}; Actual: {hash}");
                status = hash == expectedHash ? WalletStatus.Verified : WalletStatus.NotVerified;
            }

            if (status != WalletStatus.Verified && previousWallets.GetValueOrDefault(wallet.Name, WalletStatus.Verified) == WalletStatus.Verified)
            {
                notifyIcon.ShowBalloonTip(3000, "Branta", $"{wallet.Name} failed verification.", ToolTipIcon.Warning);
            }

            wallets.Add(new Wallet
            {
                Name = wallet.Name,
                Status = status
            });
        }

        return wallets
            .OrderBy(w => w.Name)
            .ToList();
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
