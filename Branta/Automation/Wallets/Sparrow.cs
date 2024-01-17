using System.IO;
using System.Text.RegularExpressions;

namespace Branta.Automation.Wallets;

public class Sparrow : BaseWallet
{
    public override string Name => "Sparrow";

    public override Dictionary<string, string> CheckSums => new()
    {
        { "1.8.1", "6b7e17b96e840aea32a40a3e73f1ba86" },
        { "1.8.0", "416e0ea8b3b6dffe097b5c3b9bd71aa6" },
        { "1.7.9", "dae54bdff194bc5aadc17b89ca50fe39" },
        { "1.7.8", "718f7b8293545395a568bbbc55671939" }
    };

    public override string GetPath()
    {
        var localPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        return Path.Join(localPath, "Sparrow");
    }

    public override string GetVersion()
    {
        var configFileContent = File.ReadAllText(Path.Join(GetPath(), "app", "Sparrow.cfg"));
        var pattern = @"-Djpackage\.app-version=(\d+\.\d+\.\d+)";
        var match = Regex.Match(configFileContent, pattern);

        if (match.Success)
        {
            return match.Groups[1].Value;
        }

        return null;
    }
}
