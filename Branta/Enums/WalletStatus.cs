namespace Branta.Enums;

public class WalletStatus
{
    public string Name { get; set; }

    public string Icon { get; set; }

    public string Color { get; set; }

    public WalletStatus(string name, string icon, string color = null)
    {
        Name = name;
        Icon = icon;
        Color = color;
    }

    public static WalletStatus None = new ("None", "");
    public static WalletStatus NotVerified = new ("Not Verified", "⚠", Enums.Color.Red);
    public static WalletStatus Verified = new ("Verified", "✓", Enums.Color.Gold);
    public static WalletStatus VersionNotSupported = new ("Version Not Supported", "⚠", Enums.Color.Red);
}