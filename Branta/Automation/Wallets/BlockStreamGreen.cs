using System.IO;

namespace Branta.Automation.Wallets;

public class BlockStreamGreen : BaseWallet
{
    public override Dictionary<string, string> CheckSums => new()
    {
        { "2.0.3", "13c8ee4a54b90c8af3e489ad72013a83" },
        { "1.2.9", "c21fcd79baeccc5cbc06101a12747322" },
        { "1.2.8", "1080a79948f976c682604bca429f6e14" },
        { "1.2.7", "6f452462209bb191386339a6b4dfef10" }
    };

    public override Dictionary<string, string> InstallerHashes => new()
    {
        { "BlockstreamGreen-x86_64.AppImage", "c8d93a090886141b5e4a1b495a399399ba89653c436769f625ff8528258b0695" },
        { "BlockstreamGreen-linux-x86_64.tar.gz", "ec51979a71c665291b3c8f25abf32845bb2f0111d6c5e05a8335a3ec5eb154b6" },
        { "BlockstreamGreen-universal.dmg", "15bf33946fd434e9d4e5179cf7830ab8a1bf98661e4ba441a83f4ce3e46ff976" },
        { "BlockstreamGreenSetup-x86_64.exe", "8d7272fd061c7f41db094e488fb001aed1aef66b48ddd92b9db721165c252c5a" },
        { "BlockstreamGreen-arm64.dmg", "1531938cad0a5f458eaa0e3dc04860622876eb2bbf2e521312735a8a1374e061" },
        { "BlockstreamGreen-x86_64.dmg", "12acba25e9a3607e72b3849bef59e6cc15fa2bae8468630daf454dcd139c91e8" },
    };

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