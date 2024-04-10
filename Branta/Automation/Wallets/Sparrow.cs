using System.IO;

namespace Branta.Automation.Wallets;

public class Sparrow : BaseWallet
{
    public override Dictionary<string, string> CheckSums => new()
    {
        { "1.8.4", "b27babb2b7e6024f58dcf975081f0285" },
        { "1.8.3", "5a4087e257ec0b2a40b4b40a8e2ed58d" },
        { "1.8.2", "25bdc8beb04642cabfef30b3529c375c" },
        { "1.8.1", "6b7e17b96e840aea32a40a3e73f1ba86" },
        { "1.8.0", "416e0ea8b3b6dffe097b5c3b9bd71aa6" },
        { "1.7.9", "dae54bdff194bc5aadc17b89ca50fe39" },
        { "1.7.8", "718f7b8293545395a568bbbc55671939" }
    };

    public Sparrow() : base("Sparrow")
    {
        InstallerName = "Sparrow";
    }

    public override string GetPath()
    {
        var localPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        return Path.Join(localPath, "Sparrow");
    }
}
