using Branta.Enums;
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

    public override Dictionary<string, string> InstallerHashes => new()
    {
        {
            "ledger-live-desktop-2.77.2-linux-x86_64.AppImage",
            "e5ee159d2030d5743a67c37be8fef56eb0ed8b78fbf33ccffbd6171c65a2bd23db58f6bca13b22334347a7dcbaf6b50d948d6d7e628789cee289bd8440e66056"
        },
        {
            "ledger-live-desktop-2.77.2-mac.dmg",
            "53dc3ca6cc5b0070a9201ff54627530b112f7a4b9a92438fa1a03c829eb09b785065492990fb423c64bb9be76b3fa0fe172cc7bf85844e952ffaf04fcacbafa9"
        },
        {
            "ledger-live-desktop-2.77.2-mac.zip",
            "d5cf5c5803f5f3d0169494b3652cd88b969d2fe489ee7b58c4388881bf0a73fbeeec16643c99852db46729bd49ed89d7c5819934c489a809e3013dd588fc0599"
        },
        {
            "ledger-live-desktop-2.77.2-win-x64.exe",
            "1e2d2684ba139f63b5c0288b7d70a4c4951e3ebcb1c7bd52b92c304dc9a13fb72aac87cf569565dd3befd16d74f74b4b7024b8977c6a1236fe5c1b1b2c271595"
        }
    };

    public Ledger() : base("Ledger Live")
    {
        InstallerName = "ledger-live";
        InstallerHashType = HashType.Sha512;
    }

    public override string GetPath()
    {
        var localPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);

        return Path.Join(localPath, "Ledger Live");
    }
}