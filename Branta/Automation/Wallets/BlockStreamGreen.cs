using System.IO;

namespace Branta.Automation.Wallets;

public class BlockStreamGreen : BaseWallet
{
    public override Dictionary<string, string> CheckSums => new()
    {
        { "1.2.9", "c21fcd79baeccc5cbc06101a12747322" },
        { "1.2.8", "1080a79948f976c682604bca429f6e14" },
        { "1.2.7", "6f452462209bb191386339a6b4dfef10" }
    };

    public BlockStreamGreen() : base("Blockstream Green")
    {
    }

    public override string GetPath()
    {
        var programFilesPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);

        return Path.Join(programFilesPath, "Blockstream", "Blockstream Green");
    }
}
