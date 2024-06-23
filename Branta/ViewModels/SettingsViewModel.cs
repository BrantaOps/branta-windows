using Branta.Classes;
using Branta.Commands;
using Branta.Stores;
using System.Collections.ObjectModel;

namespace Branta.ViewModels;

public class SettingsViewModel : BaseViewModel
{
	private bool _bitcoinAddressesEnabled;

	public bool BitcoinAddressesEnabled
	{
		get => _bitcoinAddressesEnabled;
		set
		{
			_bitcoinAddressesEnabled = value;
			OnPropertyChanged();
			SaveSettings();
		}
	}

	private bool _seedPhraseEnabled;

	public bool SeedPhraseEnabled
	{
		get => _seedPhraseEnabled;
		set
		{
			_seedPhraseEnabled = value;
			OnPropertyChanged();
			SaveSettings();
		}
	}

	private bool _extendedPublicKeyEnabled;

	public bool ExtendedPublicKeyEnabled
	{
		get => _extendedPublicKeyEnabled;
		set
		{
			_extendedPublicKeyEnabled = value;
			OnPropertyChanged();
			SaveSettings();
		}
	}

	private bool _privateKeyEnabled;

	public bool PrivateKeyEnabled
	{
		get => _privateKeyEnabled;
		set
		{
			_privateKeyEnabled = value;
			OnPropertyChanged();
			SaveSettings();
		}
	}

	private bool _nostrPublicKeyEnabled;

	public bool NostrPublicKeyEnabled
	{
		get => _nostrPublicKeyEnabled;
		set
		{
			_nostrPublicKeyEnabled = value;
			OnPropertyChanged();
			SaveSettings();
		}
	}

	private bool _nostrPrivateKeyEnabled;

	public bool NostrPrivateKeyEnabled
	{
		get => _nostrPrivateKeyEnabled;
		set
		{
			_nostrPrivateKeyEnabled = value;
			OnPropertyChanged();
			SaveSettings();
		}
	}

	private bool _launchingWalletEnabled;

	public bool LaunchingWalletEnabled
	{
		get => _launchingWalletEnabled;
		set
		{
			_launchingWalletEnabled = value;
			OnPropertyChanged();
			SaveSettings();
		}
	}

	private bool _walletStatusChangeEnabled;

	public bool WalletStatusChangeEnabled
	{
		get => _walletStatusChangeEnabled;
		set
		{
			_walletStatusChangeEnabled = value;
			OnPropertyChanged();
			SaveSettings();
		}
	}

	private DateTime? _lastUpdated;

	public DateTime? LastUpdated
	{
		get => _lastUpdated;
		set
		{
			_lastUpdated = value;
			OnPropertyChanged();
			SaveSettings();
		}
	}

	private ObservableCollection<VerifyEveryComboBoxItem> _verifyEveryOptions = [];
	public ObservableCollection<VerifyEveryComboBoxItem> VerifyEveryOptions
	{
		get { return _verifyEveryOptions; }
		set
		{
			_verifyEveryOptions = value;
			OnPropertyChanged(nameof(VerifyEveryOptions));
		}
	}

	private VerifyEveryComboBoxItem _selectedVerifyEveryOption;
	public VerifyEveryComboBoxItem SelectedVerifyEveryOption
	{
		get { return _selectedVerifyEveryOption; }
		set
		{
			_selectedVerifyEveryOption = value;
			OnPropertyChanged(nameof(SelectedVerifyEveryOption));
			SaveSettings();
		}
	}

	public LoadCheckSumsCommand LoadCheckSumsCommand { get; }
	public LoadInstallerHashesCommand LoadInstallerHashesCommand { get; }

	public WalletVerificationViewModel WalletVerificationViewModel { get; }
	public InstallerVerificationViewModel InstallerVerificationViewModel { get; }

	public readonly Settings Settings;

	private bool _settingsInitialized = false;

	public SettingsViewModel(Settings settings, CheckSumStore checkSumStore, InstallerHashStore installerHashStore, WalletVerificationViewModel walletVerificationViewModel, InstallerVerificationViewModel installerVerificationViewModel)
	{
		Settings = settings;
		LastUpdated = installerHashStore.LastUpdated > checkSumStore.LastUpdated ? checkSumStore.LastUpdated : installerHashStore.LastUpdated;
		checkSumStore.LastUpdatedEvent += date => LastUpdated = date;
		installerHashStore.LastUpdatedEvent += date => LastUpdated = date;

		WalletVerificationViewModel = walletVerificationViewModel;
		InstallerVerificationViewModel = installerVerificationViewModel;

		LoadCheckSumsCommand = new LoadCheckSumsCommand(checkSumStore);
		LoadInstallerHashesCommand = new LoadInstallerHashesCommand(installerHashStore);

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

		if (settings.WalletVerification.WalletVerifyEvery != Settings.WalletVerification.WalletVerifyEvery)
		{
			WalletVerificationViewModel.SetTimer(settings.WalletVerification.WalletVerifyEvery);
		}

		Settings.Update(settings);
	}

	private Settings GetSettings()
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
				WalletVerifyEvery = SelectedVerifyEveryOption.Value,
				LaunchingWalletEnabled = LaunchingWalletEnabled,
				WalletStatusChangeEnabled = WalletStatusChangeEnabled,
			}
		};
	}

	private void SetSettings(Settings settings)
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