using System.Windows;

namespace Branta.Enums;

public class WalletStatus(string name, string languageDictionaryName, string icon = null, string color = Color.Transparent)
{
    public string Name { private get; set; } = name;

    public string Icon { get; set; } = icon;

    public string LanguageDictionaryName { get; set; } = languageDictionaryName;

    public string Color { get; set; } = color;

    public static WalletStatus Verified = new("Verified", "VerifiedMessage", "✓", Enums.Color.Gold);
    public static WalletStatus NotVerified = new("NotVerified", "NotVerifiedMessage");
    public static WalletStatus VersionNotSupported = new("VersionNotSupported", "VersionNotSupportedMessage");
    public static WalletStatus NotFound = new("NotFound", "NotFoundMessage");
    public static WalletStatus Installing = new("Installing", "InstallingMessage");

    public string GetName(ResourceDictionary resourceDictionary)
    {
        return resourceDictionary[Name].ToString();
    }
}