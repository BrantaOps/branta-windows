using System.IO;
using Branta.Enums;

namespace Branta.Automation.Wallets;

public class Trezor : BaseWallet
{
    public override Dictionary<string, string> CheckSums => new()
    {
        { "24.3.4", "50cda1bd8576ae6c09ea1a74fc737ec6" },
        { "24.1.2", "b7870df0e3bad1a6324339ba49809f26" },
        { "24.1.1", "f28f804762ba8817acaa4cfd9dd11847" },
        { "23.12.4", "fde08eb9997ee117a81d2bd65be35b12" },
        { "23.12.3", "8247a4cd6483a910fb44e76b264ae351" },
        { "23.12.2", "98123719e05a9d004b776084aba1d133" }
    };

    public override Dictionary<string, string> InstallerHashes => new()
    {
        { "Trezor-Suite-24.3.4-win-x64.exe", "QIQ6lkTJgyzL0PU45RYV/WzywP8PaEEore4UQb6qiGWuBcTN+RhAtOlWSNCEswMz+GZUA7nfBfajU/ndjfSGBQ==" },
        { "Trezor-Suite-24.3.3-win-x64.exe", "n1n/1/SITTJzJttUXWiN9mxoL3Udyh6HVMXpimKYoibdAWvV8EDEaswn8ofiU2pcQ7F5GB/PA3c7bfaWORfTpw==" },
        { "Trezor-Suite-24.3.3-mac-x64.dmg", "vBI4c/6mSjQZjpMxybFsYFL2e/8uNtrjEzMBZNeQulsnpGnZ/9As5zXU5MYumYkFVICCxpxawIv+lrATBMJQeA==" },
        { "Trezor-Suite-24.3.3-mac-arm64.dmg", "xZ6pMgNelfXOzKxGrp8TOXNCuFj/4Poz0yEcZRdMPMm8UY08k7IUPoMkjMw3wzSY2NKc9JNCSE9K8N1qE8VK/A==" },
        { "Trezor-Suite-24.3.3-linux-arm64.AppImage", "9/yhYKwAnOTVTC/DYHR7QP5AzAKJ4MnsbxZJuKHQhoRClQmT2MK485M5+tFXI0weRP50hpx42PY8QA/njmpwFw==" }
    };

    public Trezor() : base("Trezor Suite")
    {
        InstallerName = "Trezor-Suite";
        InstallerHashType = HashType.Sha512WithBase64Encode;
    }

    public override string GetPath()
    {
        var localPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        return Path.Join(localPath, "Programs", Name);
    }
}