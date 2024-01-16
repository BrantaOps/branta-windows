using System.IO;
using System.Text.RegularExpressions;

namespace Branta.Automation.Wallets;

public class Wasabi : BaseWallet
{
    public override string Name => "Wasabi";

    public override Dictionary<string, string> CheckSums => new()
    {
        { "2.0.5", "9b0e8a5d732a862820bfec7e092707a7" } // TODO - Not Verified
    };

    public override string GetPath()
    {
        var programFilesPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);

        return Path.Join(programFilesPath, "WasabiWallet");
    }

    public override string GetVersion()
    {
        var configFileContent = File.ReadAllText(Path.Join(GetPath(), "WalletWasabi.Daemon.deps.json"));
        var pattern = $@"""{Regex.Escape("WalletWasabi.Daemon")}/(?<version>\d+\.\d+\.\d+)""";
        var match = Regex.Match(configFileContent, pattern);

        if (match.Success)
        {
            return match.Groups["version"].Value;
        }

        return null;
    }
}
