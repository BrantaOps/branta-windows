namespace Branta.Enums;

public class WalletStatus(string name, string languageDictionaryName, string icon = null, string color = Color.Transparent)
{
    public string Name { get; set; } = name;

    public string Icon { get; set; } = icon;

    public string LanguageDictionaryName { get; set; } = languageDictionaryName;

    public string Color { get; set; } = color;

    public static WalletStatus Verified = new("Verified", "VerifiedMessage", "✓", Enums.Color.Gold);
    public static WalletStatus NotVerified = new("Not Verified", "NotVerifiedMessage");
    public static WalletStatus VersionNotSupported = new("Version Not Supported", "VersionNotSupportedMessage");
    public static WalletStatus NotFound = new("Not Found", "NotFoundMessage");
}