namespace Branta.Features.Settings;

public class SettingsStore
{
    public ClipboardGuardianSettings ClipboardGuardian { get; set; } = new();
    public WalletVerificationSettings WalletVerification { get; set; } = new();

    public event Action WalletVerifyEveryChanged;

    public static SettingsStore Load()
    {
        return new SettingsStore
        {
            ClipboardGuardian = new ClipboardGuardianSettings
            {
                BitcoinAddressesEnabled = Properties.Settings.Default.BitcoinAddressesEnabled,
                SeedPhraseEnabled = Properties.Settings.Default.SeedPhraseEnabled,
                ExtendedPublicKeyEnabled = Properties.Settings.Default.ExtendedPublicKeyEnabled,
                PrivateKeyEnabled = Properties.Settings.Default.PrivateKeyEnabled,
                NostrPublicKeyEnabled = Properties.Settings.Default.NostrPublicKeyEnabled,
                NostrPrivateKeyEnabled = Properties.Settings.Default.NostrPrivateKeyEnabled
            },
            WalletVerification = new WalletVerificationSettings
            {
                WalletVerifyEvery = Properties.Settings.Default.WalletVerifyEvery,
                LaunchingWalletEnabled = Properties.Settings.Default.LaunchingWalletEnabled,
                WalletStatusChangeEnabled = Properties.Settings.Default.WalletStatusChangeEnabled
            }
        };
    }

    public static void Save(SettingsStore settings)
    {
        Properties.Settings.Default.BitcoinAddressesEnabled = settings.ClipboardGuardian.BitcoinAddressesEnabled;
        Properties.Settings.Default.SeedPhraseEnabled = settings.ClipboardGuardian.SeedPhraseEnabled;
        Properties.Settings.Default.ExtendedPublicKeyEnabled = settings.ClipboardGuardian.ExtendedPublicKeyEnabled;
        Properties.Settings.Default.PrivateKeyEnabled = settings.ClipboardGuardian.PrivateKeyEnabled;
        Properties.Settings.Default.NostrPublicKeyEnabled = settings.ClipboardGuardian.NostrPublicKeyEnabled;
        Properties.Settings.Default.NostrPrivateKeyEnabled = settings.ClipboardGuardian.NostrPrivateKeyEnabled;

        Properties.Settings.Default.WalletVerifyEvery = settings.WalletVerification.WalletVerifyEvery;
        Properties.Settings.Default.LaunchingWalletEnabled = settings.WalletVerification.LaunchingWalletEnabled;
        Properties.Settings.Default.WalletStatusChangeEnabled = settings.WalletVerification.WalletStatusChangeEnabled;

        Properties.Settings.Default.Save();
    }

    public void Update(SettingsStore settings)
    {
        var isWalletVerifyEveryChanged = WalletVerification.WalletVerifyEvery != settings.WalletVerification.WalletVerifyEvery;

        Save(settings);

        ClipboardGuardian = settings.ClipboardGuardian;
        WalletVerification = settings.WalletVerification;

        if (isWalletVerifyEveryChanged)
        {
            WalletVerifyEveryChanged?.Invoke();
        }
    }
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