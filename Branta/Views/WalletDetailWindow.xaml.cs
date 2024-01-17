using System.ComponentModel;
using Branta.Domain;
using System.Windows;
using Branta.Enums;

namespace Branta.Views;

public partial class WalletDetailWindow : Window
{
    public WalletDetailWindow(Wallet wallet)
    {
        InitializeComponent();

        TbWallet.Text = $"{wallet.Name} {wallet.Version}";

        TbInfo.Text = GetStatusMessage(wallet);
    }

    private static string GetStatusMessage(Wallet wallet)
    {
        if (wallet.Status == WalletStatus.Verified)
        {
            return $"Branta verified the validity of {wallet.Name}.";
        }

        if (wallet.Status == WalletStatus.NotVerified)
        {

            return $"Branta could not verify the validity of {wallet.Name}.";
        }

        if (wallet.Status == WalletStatus.VersionNotSupported)
        {
            return "Version not supported.";
        }

        throw new InvalidEnumArgumentException();
    }
}
