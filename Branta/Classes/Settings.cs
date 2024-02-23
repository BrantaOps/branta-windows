namespace Branta.Classes;

public class Settings
{
    public ClipboardGuardianSettings ClipboardGuardian { get; set; } = new();
    public WalletVerificationSettings WalletVerification { get; set; } = new();
}

public class ClipboardGuardianSettings
{
    public bool BitcoinAddressesEnabled { get; set; } = true;

    public bool SeedPhraseEnabled { get; set; } = true;

    public bool ExtendedPublicKeyEnabled { get; set; } = true;

    public bool PrivateKeyEnabled { get; set; } = true;

    public bool NostrPublicKeyEnabled { get; set; } = true;

    public bool NostrPrivateKeyEnabled { get; set; } = true;
}

public class WalletVerificationSettings
{
    public TimeSpan WalletVerifyEvery { get; set; } = new(0, 0, 10);

    public bool LaunchingWalletEnabled { get; set; } = true;

    public bool WalletStatusChangeEnabled { get; set; } = true;
}
