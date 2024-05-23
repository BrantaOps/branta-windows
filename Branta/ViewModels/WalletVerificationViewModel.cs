using Branta.Classes;
using Branta.Classes.Wallets;
using Branta.Commands;
using Branta.Models;
using Branta.Stores;
using System.Collections.ObjectModel;
using System.Windows;
using Timer = System.Timers.Timer;

namespace Branta.ViewModels;

public class WalletVerificationViewModel : BaseViewModel
{
    private readonly ObservableCollection<WalletViewModel> _wallets = new();
    private readonly ResourceDictionary _resourceDictionary;
    private readonly Timer _loadCheckSumsTimer;
    private readonly Timer _focusTimer;

    public List<BaseWallet> WalletTypes = new();

    private Timer _verifyWalletsTimer;

    public LoadCheckSumsCommand LoadCheckSumsCommand { get; }
    public VerifyWalletsCommand VerifyWalletsCommand { get; }
    public FocusCommand FocusCommand { get; }

    public IEnumerable<WalletViewModel> Wallets => _wallets;

    private bool _isLoading = true;

    public bool IsLoading
    {
        get => _isLoading;
        set
        {
            _isLoading = value;
            OnPropertyChanged();
        }
    }

    public WalletVerificationViewModel(Settings settings, ResourceDictionary resourceDictionary,
        FocusCommand focusCommand, LoadCheckSumsCommand loadCheckSumsCommand, VerifyWalletsCommand verifyWalletsCommand)
    {
        _resourceDictionary = resourceDictionary;

        FocusCommand = focusCommand;
        LoadCheckSumsCommand = loadCheckSumsCommand;
        VerifyWalletsCommand = verifyWalletsCommand;

        _loadCheckSumsTimer = new Timer(new TimeSpan(0, 30, 0));
        _loadCheckSumsTimer.Elapsed += (_, _) => LoadCheckSumsCommand.Execute(null);
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
            var newWallet = new WalletViewModel(wallet, _resourceDictionary);

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