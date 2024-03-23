using System.IO;

namespace Branta.Automation.Wallets;

public class Ledger : BaseWallet
{
    public override Dictionary<string, string> CheckSums => new()
    {
        { "2.77.2", "6b0acbfee50ae137e201899a44372608" },
        { "2.77.1", "b0b40c5a481dc430f06264de6fd40239" },
        { "2.75.0", "833bfb66155d24510c71be62bf341d83" }
    };

    public override Dictionary<string, string> InstallerHashes => new();

    public Ledger() : base("Ledger Live")
    {
    }

    public override string GetPath()
    {
        var localPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);

        return Path.Join(localPath, "Ledger Live");
    }
}
