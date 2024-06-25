using Branta.Classes;
using Branta.Classes.Wallets;
using Branta.Commands;
using Branta.Models;
using Branta.Stores;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using Timer = System.Timers.Timer;

namespace Branta.ViewModels;

public partial class WalletVerificationViewModel : ObservableObject
{
    private readonly ObservableCollection<WalletViewModel> _wallets = new();
    private readonly LanguageStore _languageStore;
    private readonly Timer _loadCheckSumsTimer;
    private readonly Timer _focusTimer;

    public List<BaseWalletType> WalletTypes = new();

    private Timer _verifyWalletsTimer;

    public LoadCheckSumsCommand LoadCheckSumsCommand { get; }
    public VerifyWalletsCommand VerifyWalletsCommand { get; }
    public FocusCommand FocusCommand { get; }

    private Dispatcher DispatchHelper => System.Windows.Application.Current.Dispatcher;

    public IEnumerable<WalletViewModel> Wallets => _wallets;

    [ObservableProperty]
    private bool _isLoading = true;

    public WalletVerificationViewModel(Settings settings, LanguageStore languageStore,
    FocusCommand focusCommand, LoadCheckSumsCommand loadCheckSumsCommand, VerifyWalletsCommand verifyWalletsCommand)
    {
        _languageStore = languageStore;

        FocusCommand = focusCommand;
        LoadCheckSumsCommand = loadCheckSumsCommand;
        VerifyWalletsCommand = verifyWalletsCommand;

        _loadCheckSumsTimer = new Timer(new TimeSpan(0, 30, 0));
        _loadCheckSumsTimer.Elapsed += (_, _) => LoadCheckSumsCommand.Execute(this);
        _loadCheckSumsTimer.Start();

        SetTimer(settings.WalletVerification.WalletVerifyEvery);

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

    public void SetTimer(TimeSpan interval)
    {
        _verifyWalletsTimer?.Dispose();

        _verifyWalletsTimer = new Timer(interval);
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
}