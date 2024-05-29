using System.IO;
using System.Text.RegularExpressions;

namespace Branta.Classes.Wallets;

public class BitcoinCore : BaseWalletType
{
    public BitcoinCore() : base("Bitcoin Core", "bitcoin-qt")
    {
        InstallerRegex = new Regex(@"bitcoin-(\d+\.\d+)-win64-setup");
    }

    public override string GetPath()
    {
        var localPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);

        return Path.Join(localPath, "Bitcoin");
    }
}