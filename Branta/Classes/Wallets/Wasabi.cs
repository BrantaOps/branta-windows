using System.IO;

namespace Branta.Classes.Wallets;

public class Wasabi : BaseWallet
{
    public Wasabi() : base("WasabiWallet", "wassabee")
    {
        InstallerName = "Wasabi";
    }

    public override string GetPath()
    {
        var programFilesPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);

        return Path.Join(programFilesPath, Name);
    }
}