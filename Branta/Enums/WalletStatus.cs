namespace Branta.Enums;

public class WalletStatus(string name, string icon, string color = null)
{
    public string Name { get; set; } = name;

    public string Icon { get; set; } = icon;

    public string Color { get; set; } = color;

    public static WalletStatus None = new ("None", "");
    public static WalletStatus NotVerified = new ("Not Verified", "⚠", Enums.Color.Red);
    public static WalletStatus Verified = new ("Verified", "✓", Enums.Color.Gold);
    public static WalletStatus VersionNotSupported = new ("Version Not Supported", "⚠", Enums.Color.Red);
}