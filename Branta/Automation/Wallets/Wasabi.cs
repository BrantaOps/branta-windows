using System.IO;

namespace Branta.Automation.Wallets;

public class Wasabi : BaseWallet
{
    public override Dictionary<string, string> CheckSums => new()
    {
        { "2.0.5", "9b0e8a5d732a862820bfec7e092707a7" },
        { "2.0.4.1", "06462137cb7968fc0a2e36fade6e6b52" },
        { "2.0.4", "ebcd119a10e793d133c52e4463ce3246" },
        { "2.0.3", "c4cc9b9f99b8e5114090c9820695d573" }
    };

    public override Dictionary<string, string> InstallerHashes => new();

    public Wasabi() : base("WasabiWallet", "wassabee")
    {
    }

    public override string GetPath()
    {
        var programFilesPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);

        return Path.Join(programFilesPath, Name);
    }
}
