using System.IO;

namespace Branta.Automation.Wallets;

public class Sparrow : BaseWallet
{
    public Sparrow() : base("Sparrow")
    {
        InstallerName = "Sparrow";
    }

    public override string GetPath()
    {
        var localPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        return Path.Join(localPath, "Sparrow");
    }
}
