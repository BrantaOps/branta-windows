using System.Windows.Controls;
using System.Windows.Input;
using Branta.Classes;
using Branta.Enums;
using Branta.Models;

namespace Branta.Windows;

public partial class WalletDetailWindow
{
    public WalletDetailWindow(Wallet wallet)
    {
        InitializeComponent();

        var dictionary = SetLanguageDictionary();

        TbLink.Foreground = Color.Brush(Color.Gold);

        TbWallet.Text = $"{wallet.Name} {wallet.Version}";

        if (!dictionary.Contains(wallet.Status.LanguageDictionaryName))
        {
            return;
        }

        var content = string.Format(dictionary[wallet.Status.LanguageDictionaryName]?.ToString() ?? string.Empty, wallet.Name);

        IcMessages.ItemsSource = content.Split("_NL_").ToList();
    }

    private void Link_Click(object sender, MouseButtonEventArgs e)
    {
        var textBlock = (TextBlock)sender;

        Helper.OpenLink(textBlock.Text);
    }
}