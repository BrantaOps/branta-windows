using System.IO;
using Branta.Enums;

namespace Branta.Automation.Wallets;

public class Trezor : BaseWallet
{
    public override Dictionary<string, string> CheckSums => new()
    {
        { "24.3.4", "50cda1bd8576ae6c09ea1a74fc737ec6" },
        { "24.1.2", "b7870df0e3bad1a6324339ba49809f26" },
        { "24.1.1", "f28f804762ba8817acaa4cfd9dd11847" },
        { "23.12.4", "fde08eb9997ee117a81d2bd65be35b12" },
        { "23.12.3", "8247a4cd6483a910fb44e76b264ae351" },
        { "23.12.2", "98123719e05a9d004b776084aba1d133" }
    };

    public Trezor() : base("Trezor Suite")
    {
        InstallerName = "Trezor-Suite";
        InstallerHashType = HashType.Sha512WithBase64Encode;
    }

    public override string GetPath()
    {
        var localPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        return Path.Join(localPath, "Programs", Name);
    }
}