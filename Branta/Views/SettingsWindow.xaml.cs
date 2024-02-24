using System.Windows.Controls;
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

    public TimeSpan VerifyEvery { get; set; }

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

        var verifyEveryOptions = new List<TimeSpan>
        {
            new(0, 0, 1),
            new(0, 0, 5),
            new(0, 0, 10),
            new(0, 0, 30),
            new(0, 1, 0),
            new(0, 5, 0),
            new(0, 10, 0),
            new(0, 30, 0)
        };

        foreach (var option in verifyEveryOptions)
        {
            ComboBoxVerifyEvery.Items.Add(new ComboBoxItem
            {
                IsSelected = option == settings.WalletVerification.WalletVerifyEvery,
                Content = option.Format(),
                Tag = option
            });
        }
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
                WalletVerifyEvery = VerifyEvery,
                LaunchingWalletEnabled = LaunchingWalletEnabled,
                WalletStatusChangeEnabled = WalletStatusChangeEnabled,
            }
        };
    }

    private void ComboBoxVerifyEvery_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var comboBox = (ComboBox) sender;
        var comboBoxItem = (ComboBoxItem) comboBox.SelectedValue;

        VerifyEvery = (TimeSpan) comboBoxItem.Tag;
    }
}
