using Branta.Enums;
using Branta.Models;

namespace Branta.ViewModels;

public class WalletViewModel : BaseViewModel
{
    private readonly Wallet _wallet;

    public string Name => _wallet.Name;

    public string DisplayName => $"{_wallet.Name}: {_wallet.Status.Name}";

    public string Version => _wallet.Version;

    public WalletStatus Status => _wallet.Status;

    public string LastScanned => _wallet.LastScanned.ToString("T");

    public bool IsNetworkActivityEnabled => _wallet.Status != WalletStatus.NotFound;

    public Wallet Wallet => _wallet;

    public WalletViewModel(Wallet wallet)
    {
        _wallet = wallet;
    }
}
