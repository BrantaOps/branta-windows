using Branta.Classes;
using Branta.Classes.Wallets;
using Branta.Commands;
using Branta.Models;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Timers;
using System.Windows;
using Timer = System.Timers.Timer;

namespace Branta.ViewModels;

public class WalletVerificationViewModel : BaseViewModel
{
    private ObservableCollection<WalletViewModel> _wallets = new();

    private readonly ResourceDictionary _resourceDictioanry;
    private readonly Timer _loadCheckSumsTimer;
    private readonly Timer _focusTimer;

    public List<BaseWallet> WalletTypes = new();

    private Timer _verifyWalletsTimer;

    public LoadCheckSumsCommand LoadCheckSumsCommand { get; }
    public VerifyWalletsCommand VerifyWalletsCommand { get; }
    public FocusCommand FocusCommand { get; }

    public IEnumerable<WalletViewModel> Wallets => _wallets;

    public string WalletsDetected
    {
        get
        {
            var resourceName = _wallets.Count == 1 ? "WalletDetected" : "WalletDetectedPlural";
            return _wallets.Count + " " + _resourceDictioanry[resourceName] + ".";
        }
    }

    private bool _isLoading = true;
    public bool IsLoading
    {
        get
        {
            return _isLoading;
        }
        set
        {
            _isLoading = value;
            OnPropertyChanged(nameof(IsLoading));
        }
    }

    public WalletVerificationViewModel(NotificationCenter notificationCenter, Settings settings, ResourceDictionary resourceDictionary)
    {
        _resourceDictioanry = resourceDictionary;
        _wallets.CollectionChanged += (object _, NotifyCollectionChangedEventArgs _) => OnPropertyChanged(nameof(WalletsDetected));

        LoadCheckSumsCommand = new LoadCheckSumsCommand(this);
        VerifyWalletsCommand = new VerifyWalletsCommand(this, notificationCenter, settings);
        FocusCommand = new FocusCommand(this, notificationCenter, settings);

        _loadCheckSumsTimer = new Timer(new TimeSpan(0, 30, 0));
        _loadCheckSumsTimer.Elapsed += (object sender, ElapsedEventArgs e) => LoadCheckSumsCommand.Execute(null);
        _loadCheckSumsTimer.Start();

        SetTimer(settings.WalletVerification.WalletVerifyEvery);

        _focusTimer = new Timer(new TimeSpan(0, 0, 2));
        _focusTimer.Elapsed += (object sender, ElapsedEventArgs e) => FocusCommand.Execute(null);
        _focusTimer.Start();


        Task.Run(async () =>
        {
            await LoadCheckSumsCommand.ExecuteAsync(null);
            VerifyWalletsCommand.Execute(null);
            FocusCommand.SetWallets(WalletTypes);
        });
    }

    public void SetTimer(TimeSpan interval)
    {
        _verifyWalletsTimer?.Dispose();

        _verifyWalletsTimer = new Timer(interval);
        _verifyWalletsTimer.Elapsed += (object sender, ElapsedEventArgs e) => VerifyWalletsCommand.Execute(null);
        _verifyWalletsTimer.Start();
    }

    public void AddWallet(Wallet wallet)
    {
        DispatchHelper.BeginInvoke(() => _wallets.Add(new WalletViewModel(wallet)));
    }

    public void ClearWallets()
    {
        DispatchHelper.BeginInvoke(() => _wallets.Clear());
    }
}
