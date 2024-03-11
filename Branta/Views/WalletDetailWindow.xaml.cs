using Branta.Classes;
using Branta.Domain;
using Branta.Enums;
using System.ComponentModel;
using System.Windows;

namespace Branta.Views;

public partial class WalletDetailWindow : BaseWindow
{
    private readonly ResourceDictionary _dictionary;

    public WalletDetailWindow(Wallet wallet)
    {
        InitializeComponent();

        SetLanguageDictionary();
        _dictionary = Resources.MergedDictionaries.FirstOrDefault();

        TbWallet.Text = $"{wallet.Name} {wallet.Version}";

        TbInfo.Text = GetStatusMessage(wallet);
    }

    private string GetStatusMessage(Wallet wallet)
    {
        if (wallet.Status == WalletStatus.Verified)
        {
            return $"{_dictionary["VerifiedMessage"]} {wallet.Name}.";
        }

        if (wallet.Status == WalletStatus.NotVerified)
        {

            return $"{_dictionary["NotVerifiedMessage"]} {wallet.Name}.";
        }

        if (wallet.Status == WalletStatus.VersionNotSupported)
        {
            return _dictionary["VersionNotSupportedMessage"]?.ToString();
        }

        throw new InvalidEnumArgumentException();
    }
}
