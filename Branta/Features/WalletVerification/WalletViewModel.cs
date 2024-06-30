using Branta.Enums;
using Branta.Features.Main;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Branta.Features.WalletVerification;

public class WalletViewModel : ObservableObject
{
    private readonly Wallet _wallet;
    public LanguageStore LanguageStore;

    public string Name => _wallet.Name;

    public string DisplayName => $"{_wallet.Name}: {_wallet.Status.GetName(LanguageStore)}";

    public string Version => _wallet.Version;

    public WalletStatus Status => _wallet.Status;

    public string LastScanned => _wallet.LastScanned.ToString("T");

    public bool IsNetworkActivityEnabled =>
        _wallet.Status != WalletStatus.NotFound && _wallet.Status != WalletStatus.Installing;

    public Wallet Wallet => _wallet;

    public WalletViewModel(Wallet wallet, LanguageStore languageStore)
    {
        _wallet = wallet;
        LanguageStore = languageStore;
    }
}