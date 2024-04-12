using System.IO;

namespace Branta.Automation.Wallets;

public class BlockStreamGreen : BaseWallet
{
    public BlockStreamGreen() : base("Blockstream Green")
    {
        InstallerName = "BlockstreamGreen";
    }

    public override string GetPath()
    {
        var programFilesPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);

        return Path.Join(programFilesPath, "Blockstream", "Blockstream Green");
    }
}