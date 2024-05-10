using Branta.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Branta.Views;

public partial class WalletVerificationView : UserControl
{
    public WalletVerificationView()
    {
        InitializeComponent();
    }

    public void OnClick_Status(object sender, MouseButtonEventArgs e)
    {
        var textBlock = (TextBlock)sender;
        var wallet = (WalletViewModel)textBlock.Tag;

        var walletDetailWindow = new WalletDetailWindow(wallet.Wallet)
        {
            Owner = Window.GetWindow(this),
            WindowStartupLocation = WindowStartupLocation.CenterOwner
        };

        walletDetailWindow.Show();
    }

    private void OnClick_NetworkActivityDetails(object sender, MouseButtonEventArgs e)
    {
        var textBlock = (TextBlock)sender;
        var wallet = (WalletViewModel)textBlock.Tag;

        var networkActivityWindow = new NetworkActivityWindow(wallet.Wallet)
        {
            Owner = Window.GetWindow(this),
            WindowStartupLocation = WindowStartupLocation.CenterOwner
        };

        networkActivityWindow.Show();
    }
}
