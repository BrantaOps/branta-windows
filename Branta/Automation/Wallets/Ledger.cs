using Branta.Enums;
using System.IO;

namespace Branta.Automation.Wallets;

public class Ledger : BaseWallet
{
    public Ledger() : base("Ledger Live")
    {
        InstallerName = "ledger-live";
        InstallerHashType = HashType.Sha512;
    }

    public override string GetPath()
    {
        var localPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);

        return Path.Join(localPath, "Ledger Live");
    }
}