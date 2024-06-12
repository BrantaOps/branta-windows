using Branta.ViewModels;
using Branta.Windows;
using System.Windows;
using System.Windows.Controls;

namespace Branta.Views;

public partial class WalletVerificationView : UserControl
{
    public WalletVerificationView()
    {
        InitializeComponent();
    }

    public void OnClick_Status(object sender, RoutedEventArgs e)
    {
        var button = (Button)sender;
        var wallet = (WalletViewModel)button.Tag;

        var walletDetailWindow = new WalletDetailWindow(wallet.Wallet, wallet.LanguageStore)
        {
            Owner = Window.GetWindow(this),
            WindowStartupLocation = WindowStartupLocation.CenterOwner
        };

        walletDetailWindow.Show();
    }
}
