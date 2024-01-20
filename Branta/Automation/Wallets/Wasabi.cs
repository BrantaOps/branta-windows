using System.IO;
using System.Text.RegularExpressions;

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

    public Wasabi() : base("Wasabi")
    {
    }

    public override string GetPath()
    {
        var programFilesPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);

        return Path.Join(programFilesPath, "WasabiWallet");
    }

    public override string GetVersion()
    {
        // Wasabi 2.0.3 uses a different deps.json file name
        var fileNames = new[] {"WalletWasabi.Daemon.deps.json", "WalletWasabi.Fluent.Desktop.deps.json"};
        var propertyPrefixes = new[] {"WalletWasabi.Daemon", "WalletWasabi.Fluent.Desktop"};

        for (var i = 0; i < fileNames.Length; i++)
        {
            try
            {
                var configFileContent = File.ReadAllText(Path.Join(GetPath(), fileNames[i]));
                var pattern = $@"""{Regex.Escape(propertyPrefixes[i])}/(?<version>\d+(\.\d+)+)""";
                var match = Regex.Match(configFileContent, pattern);

                if (match.Success)
                {
                    return match.Groups["version"].Value;
                }
            }
            catch
            {
                // ignored
            }
        }

        return null;
    }
}
