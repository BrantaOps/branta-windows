using Branta.Classes;

namespace Branta.Views;

public partial class SettingsWindow : BaseWindow
{
    private bool  _bitcoinAddressesEnabled;
    public bool  BitcoinAddressesEnabled
    {
        get => _bitcoinAddressesEnabled;
        set { _bitcoinAddressesEnabled = value; OnPropertyChanged(); }
    }

    private bool _seedPhraseEnabled;
    public bool  SeedPhraseEnabled
    {
        get => _seedPhraseEnabled;
        set { _seedPhraseEnabled = value; OnPropertyChanged(); }
    }

    private bool _extendedPublicKeyEnabled;
    public bool  ExtendedPublicKeyEnabled
    {
        get => _extendedPublicKeyEnabled;
        set { _extendedPublicKeyEnabled = value; OnPropertyChanged(); }
    }

    private bool _privateKeyEnabled;
    public bool  PrivateKeyEnabled
    {
        get => _privateKeyEnabled;
        set { _privateKeyEnabled = value; OnPropertyChanged(); }
    }

    private bool _nostrPublicKeyEnabled;
    public bool NostrPublicKeyEnabled
    {
        get => _nostrPublicKeyEnabled;
        set { _nostrPublicKeyEnabled = value; OnPropertyChanged(); }
    }

    private bool _nostrPrivateKeyEnabled;
    public bool NostrPrivateKeyEnabled
    {
        get => _nostrPrivateKeyEnabled;
        set { _nostrPrivateKeyEnabled = value; OnPropertyChanged(); }
    }

    private bool _launchingWalletEnabled;
    public bool LaunchingWalletEnabled
    {
        get => _launchingWalletEnabled;
        set { _launchingWalletEnabled = value; OnPropertyChanged(); }
    }

    private bool _walletStatusChangeEnabled;

    public bool WalletStatusChangeEnabled
    {
        get => _walletStatusChangeEnabled;
        set { _walletStatusChangeEnabled = value; OnPropertyChanged(); }
    }


    public SettingsWindow(Settings settings)
    {
        InitializeComponent();
        DataContext = this;

        BitcoinAddressesEnabled = settings.ClipboardGuardian.BitcoinAddressesEnabled;
        SeedPhraseEnabled = settings.ClipboardGuardian.SeedPhraseEnabled;
        ExtendedPublicKeyEnabled = settings.ClipboardGuardian.ExtendedPublicKeyEnabled;
        PrivateKeyEnabled = settings.ClipboardGuardian.PrivateKeyEnabled;
        NostrPublicKeyEnabled = settings.ClipboardGuardian.NostrPublicKeyEnabled;
        NostrPrivateKeyEnabled = settings.ClipboardGuardian.NostrPrivateKeyEnabled;
        NostrPrivateKeyEnabled = settings.ClipboardGuardian.NostrPrivateKeyEnabled;

        LaunchingWalletEnabled = settings.WalletVerification.LaunchingWalletEnabled;
        WalletStatusChangeEnabled = settings.WalletVerification.WalletStatusChangeEnabled;
    }

    public Settings GetSettings()
    {
        return new Settings
        {
            ClipboardGuardian = new ClipboardGuardianSettings
            {
                BitcoinAddressesEnabled = BitcoinAddressesEnabled,
                SeedPhraseEnabled = SeedPhraseEnabled,
                ExtendedPublicKeyEnabled = ExtendedPublicKeyEnabled,
                PrivateKeyEnabled = PrivateKeyEnabled,
                NostrPublicKeyEnabled = NostrPublicKeyEnabled,
                NostrPrivateKeyEnabled = NostrPrivateKeyEnabled
            },
            WalletVerification = new WalletVerificationSettings
            {
                LaunchingWalletEnabled = LaunchingWalletEnabled,
                WalletStatusChangeEnabled = WalletStatusChangeEnabled,
            }
        };
    }
}
