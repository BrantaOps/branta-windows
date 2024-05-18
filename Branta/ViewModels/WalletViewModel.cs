using Branta.Enums;
using Branta.Models;
using System.Windows;

namespace Branta.ViewModels;

public class WalletViewModel : BaseViewModel
{
    private readonly Wallet _wallet;
    private readonly ResourceDictionary _resourceDictionary;

    public string Name => _wallet.Name;

    public string DisplayName => $"{_wallet.Name}: {_wallet.Status.GetName(_resourceDictionary)}";

    public string Version => _wallet.Version;

    public WalletStatus Status => _wallet.Status;

    public string LastScanned => _wallet.LastScanned.ToString("T");

    public bool IsNetworkActivityEnabled => _wallet.Status != WalletStatus.NotFound && _wallet.Status != WalletStatus.Installing;

    public Wallet Wallet => _wallet;

    public WalletViewModel(Wallet wallet, ResourceDictionary resourceDictionary)
    {
        _wallet = wallet;
        _resourceDictionary = resourceDictionary;
    }
}
