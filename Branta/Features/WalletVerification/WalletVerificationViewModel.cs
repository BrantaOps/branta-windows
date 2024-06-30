using Branta.Classes.Wallets;
using Branta.Features.Main;
using Branta.Features.Settings;
using Branta.Features.WalletFocus;
using Branta.Features.WalletVerification;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using Timer = System.Timers.Timer;

namespace Branta.ViewModels;

public partial class WalletVerificationViewModel : ObservableObject
{
    private readonly SettingsStore _settings;
    private readonly LanguageStore _languageStore;
    private readonly CheckSumStore _checkSumStore;
    private readonly Timer _loadCheckSumsTimer;
    private readonly Timer _focusTimer;

    private readonly ObservableCollection<WalletViewModel> _wallets = new();

    public List<BaseWalletType> WalletTypes = new();

    private Timer _verifyWalletsTimer;

    [RelayCommand]
    public async Task LoadCheckSums()
    {
        await _checkSumStore.LoadAsync();
    }

    public VerifyWalletsCommand VerifyWalletsCommand { get; }
    public FocusCommand FocusCommand { get; }

    private Dispatcher DispatchHelper => System.Windows.Application.Current.Dispatcher;

    public IEnumerable<WalletViewModel> Wallets => _wallets;

    [ObservableProperty]
    private bool _isLoading = true;

    public WalletVerificationViewModel(SettingsStore settings, LanguageStore languageStore, FocusCommand focusCommand, CheckSumStore checkSumStore, VerifyWalletsCommand verifyWalletsCommand)
    {
        _settings = settings;
        _languageStore = languageStore;
        _checkSumStore = checkSumStore;

        FocusCommand = focusCommand;
        VerifyWalletsCommand = verifyWalletsCommand;

        _checkSumStore.CheckSumsChanged += On_CheckSumsChanged;

        _loadCheckSumsTimer = new Timer(new TimeSpan(0, 30, 0));
        _loadCheckSumsTimer.Elapsed += (_, _) => LoadCheckSumsCommand.Execute(this);
        _loadCheckSumsTimer.Start();

        SetTimer();
        settings.WalletVerifyEveryChanged += SetTimer;

        _focusTimer = new Timer(new TimeSpan(0, 0, 2));
        _focusTimer.Elapsed += (_, _) => FocusCommand.Execute(null);
        _focusTimer.Start();

        Task.Run(async () =>
        {
            await LoadCheckSumsCommand.ExecuteAsync(this);
            VerifyWalletsCommand.Execute(this);
            FocusCommand.SetWallets(WalletTypes);
        });
    }

    public void SetTimer()
    {
        _verifyWalletsTimer?.Dispose();

        _verifyWalletsTimer = new Timer(_settings.WalletVerification.WalletVerifyEvery);
        _verifyWalletsTimer.Elapsed += (_, _) => VerifyWalletsCommand.Execute(this);
        _verifyWalletsTimer.Start();
    }

    public void AddWallet(Wallet wallet)
    {
        DispatchHelper.BeginInvoke(() =>
        {
            var originalWallet = _wallets.FirstOrDefault(w => w.Name == wallet.Name);
            var newWallet = new WalletViewModel(wallet, _languageStore);

            if (originalWallet == null)
            {
                _wallets.Add(newWallet);
            }
            else
            {
                var index = _wallets.IndexOf(originalWallet);
                _wallets[index] = newWallet;
            }
        });
    }

    private void On_CheckSumsChanged()
    {
        WalletTypes = _checkSumStore.WalletTypes;
        IsLoading = false;
    }
}