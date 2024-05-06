using Branta.Enums;
using System.IO;

namespace Branta.Automation.Wallets;

public class Trezor : BaseWallet
{
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