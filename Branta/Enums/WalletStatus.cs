using Ardalis.SmartEnum;

namespace Branta.Enums;

public sealed class WalletStatus(int value, string name, string icon, Color color) : SmartEnum<WalletStatus>(name, value)
{
    public static readonly WalletStatus NotVerified = new (0, "Not Verified", "⚠", Color.Red);
    public static readonly WalletStatus Verified = new (1, "Verified", "✓", Color.Gold);
    public static readonly WalletStatus VersionNotSupported = new (2, "Version Not Supported", "⚠", Color.Blue);

    public string Icon { get; set; } = icon;

    public Color Color { get; set; } = color;
}