using Branta.Classes;
using Branta.Features.InstallerVerification;
using Branta.Features.Settings;
using Branta.Features.WalletVerification;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace Branta.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    private readonly CheckSumStore _checkSumStore;
    private readonly InstallerHashStore _installerHashStore;
    
    public readonly SettingsStore Settings;

    private bool _settingsInitialized = false;

    [ObservableProperty]
    private bool _bitcoinAddressesEnabled;

    partial void OnBitcoinAddressesEnabledChanged(bool value)
    {
        SaveSettings();
    }

    [ObservableProperty]
    private bool _seedPhraseEnabled;

    partial void OnSeedPhraseEnabledChanged(bool value)
    {
        SaveSettings();
    }

    [ObservableProperty]
    private bool _extendedPublicKeyEnabled;

    partial void OnExtendedPublicKeyEnabledChanged(bool value)
    {
        SaveSettings();
    }

    [ObservableProperty]
    private bool _privateKeyEnabled;

    partial void OnPrivateKeyEnabledChanged(bool value)
    {
        SaveSettings();
    }

    [ObservableProperty]
    private bool _nostrPublicKeyEnabled;

    partial void OnNostrPublicKeyEnabledChanged(bool value)
    {
        SaveSettings();
    }

    [ObservableProperty]
    private bool _nostrPrivateKeyEnabled;

    partial void OnPrivateKeyEnabledChanging(bool value)
    {
        SaveSettings();
    }

    [ObservableProperty]
    private bool _launchingWalletEnabled;

    partial void OnLaunchingWalletEnabledChanging(bool value)
    {
        SaveSettings();
    }

    [ObservableProperty]
    private bool _walletStatusChangeEnabled;

    partial void OnWalletStatusChangeEnabledChanging(bool value)
    {
        SaveSettings();
    }

    [ObservableProperty]
    private ObservableCollection<VerifyEveryComboBoxItem> _verifyEveryOptions = [];

    [ObservableProperty]
    private VerifyEveryComboBoxItem _selectedVerifyEveryOption;

    partial void OnSelectedVerifyEveryOptionChanged(VerifyEveryComboBoxItem value)
    {
        SaveSettings();
    }

    [ObservableProperty]
    private DateTime? _lastUpdated;

    [RelayCommand]
    public async Task Refresh()
    {
        await _checkSumStore.LoadAsync();
        await _installerHashStore.LoadAsync();
    }

    public SettingsViewModel(SettingsStore settings, CheckSumStore checkSumStore, InstallerHashStore installerHashStore)
    {
        _checkSumStore = checkSumStore;
        _installerHashStore = installerHashStore;
        Settings = settings;

        LastUpdated = installerHashStore.LastUpdated > checkSumStore.LastUpdated ? checkSumStore.LastUpdated : installerHashStore.LastUpdated;
        _checkSumStore.CheckSumsChanged += () => LastUpdated = _checkSumStore.LastUpdated;

        SetSettings(settings);

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
        }.Select(ts => new VerifyEveryComboBoxItem()
                {
                DisplayName = ts.Format(),
                Value = ts
                });

        foreach (var option in verifyEveryOptions)
        {
            if (option.Value == settings.WalletVerification.WalletVerifyEvery)
            {
                _selectedVerifyEveryOption = option;
            }

            _verifyEveryOptions.Add(option);
        }
    }

    public void SaveSettings()
    {
        if (!_settingsInitialized)
        {
            return;
        }

        var settings = GetSettings();
        Settings.Update(settings);
    }

    private SettingsStore GetSettings()
    {
        return new SettingsStore
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
                WalletVerifyEvery = SelectedVerifyEveryOption.Value,
                LaunchingWalletEnabled = LaunchingWalletEnabled,
                WalletStatusChangeEnabled = WalletStatusChangeEnabled,
            }
        };
    }

    private void SetSettings(SettingsStore settings)
    {
        BitcoinAddressesEnabled = settings.ClipboardGuardian.BitcoinAddressesEnabled;
        SeedPhraseEnabled = settings.ClipboardGuardian.SeedPhraseEnabled;
        ExtendedPublicKeyEnabled = settings.ClipboardGuardian.ExtendedPublicKeyEnabled;
        PrivateKeyEnabled = settings.ClipboardGuardian.PrivateKeyEnabled;
        NostrPublicKeyEnabled = settings.ClipboardGuardian.NostrPublicKeyEnabled;
        NostrPrivateKeyEnabled = settings.ClipboardGuardian.NostrPrivateKeyEnabled;
        NostrPrivateKeyEnabled = settings.ClipboardGuardian.NostrPrivateKeyEnabled;

        LaunchingWalletEnabled = settings.WalletVerification.LaunchingWalletEnabled;
        WalletStatusChangeEnabled = settings.WalletVerification.WalletStatusChangeEnabled;

        _settingsInitialized = true;
    }
}