using Ardalis.SmartEnum;

namespace Branta.Enums;

public sealed class WalletStatus(int value, string name, string icon, Color color = null) : SmartEnum<WalletStatus>(name, value)
{
    public static readonly WalletStatus None = new (0, "None", "");
    public static readonly WalletStatus NotVerified = new (1, "Not Verified", "⚠", Color.Red);
    public static readonly WalletStatus Verified = new (2, "Verified", "✓", Color.Gold);
    public static readonly WalletStatus VersionNotSupported = new (3, "Version Not Supported", "⚠", Color.Red);

    public string Icon { get; set; } = icon;

    public Color Color { get; set; } = color;
}