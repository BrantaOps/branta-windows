using System.IO;

namespace Branta.Automation.Wallets;

public class Wasabi : BaseWallet
{
    public override Dictionary<string, string> CheckSums => new()
    {
        { "2.0.6", "8424d1148164565b296f87e5abc49885" },
        { "2.0.5", "9b0e8a5d732a862820bfec7e092707a7" },
        { "2.0.4.1", "06462137cb7968fc0a2e36fade6e6b52" },
        { "2.0.4", "ebcd119a10e793d133c52e4463ce3246" },
        { "2.0.3", "c4cc9b9f99b8e5114090c9820695d573" }
    };

    public override Dictionary<string, string> InstallerHashes => new()
    {
        { "Wasabi-2.0.6-arm64.dmg", "72590adf8928a84ef1dadadafbd892c5d71b28ebd9d32d8de4665bfe173bbc76" },
        { "Wasabi-2.0.6-linux-x64.zip", "93097fd0b6abe2064d6391a7f67d0e1b738c615929c4fef4ba7e4269ee2d6b9e" },
        { "Wasabi-2.0.6-macOS-arm64.zip", "2489cb0c0e5a12461aaba39bb1a39dcde87cc76f7e98b332248f426059808e74" },
        { "Wasabi-2.0.6-macOS-x64.zip", "cde9ce7983465c76a94accf5a1f9c52324377225c4990997c5575c4c2ab58bba" },
        { "Wasabi-2.0.6-win-x64.zip", "88cd668a118a08c7104734cf3c4bccb9dcb8ffe87252db2b4ef2ff755014e545" },
        { "Wasabi-2.0.6.deb", "9c3cd5ede82136915fe98057fc4ca34cf3636a6c0954cc7c22b12e2cc404a26b" },
        { "Wasabi-2.0.6.dmg", "30387d0f944a6e1feb080541eb6a9b86ee97515476624615fae1a78cbcd09c61" },
        { "Wasabi-2.0.6.msi", "23c0cdd69a3cf3580383cc161bdb0a88ec1b21c995118bdf3c3197358c386aba" },
        { "Wasabi-2.0.6.tar.gz", "571b50645b225047823045a4767984f715c5a1aedf59f3960b65319d964089ad" }
    };

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