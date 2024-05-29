using Branta.Classes;
using Branta.Enums;
using Branta.Models;
using Branta.Stores;
using System.Windows.Controls;
using System.Windows.Input;

namespace Branta.Windows;

public partial class WalletDetailWindow
{
    public WalletDetailWindow(Wallet wallet, LanguageStore languageStore)
    {
        InitializeComponent();

        SetLanguageDictionary(languageStore);

        TbLink.Foreground = Color.Brush(Color.Gold);

        TbWallet.Text = $"{wallet.Name} {wallet.Version}";

        var content = languageStore.Format(wallet.Status.LanguageDictionaryName, wallet.Name);

        IcMessages.ItemsSource = content.Split("_NL_").ToList();
    }

    private void Link_Click(object sender, MouseButtonEventArgs e)
    {
        var textBlock = (TextBlock)sender;

        Helper.OpenLink(textBlock.Text);
    }
}