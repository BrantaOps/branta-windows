using System.IO;
using System.Text.RegularExpressions;

namespace Branta.Classes.Wallets;

public class Sparrow : BaseWallet
{
    public Sparrow() : base("Sparrow")
    {
        InstallerRegex = new Regex(@"Sparrow-(\d+\.\d+\.\d+)");
    }

    public override string GetPath()
    {
        var localPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        return Path.Join(localPath, "Sparrow");
    }
}
