using System.IO;
using System.Text.RegularExpressions;

namespace Branta.Automation.Wallets;

public class Sparrow : BaseWallet
{
    public override string Name => "Sparrow";

    public override Dictionary<string, string> CheckSums => new()
    {
        { "1.8.1", "6b7e17b96e840aea32a40a3e73f1ba86" } // TODO - Not Verified
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
