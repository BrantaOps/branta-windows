using Branta.Classes;
using Branta.Enums;
using Branta.Features.Main;
using System.Windows.Controls;
using System.Windows.Input;

namespace Branta.Features.WalletVerification;

public partial class WalletDetailWindow
{
    public WalletDetailWindow(Wallet wallet, LanguageStore languageStore)
    {
        InitializeComponent();

        this.SetLanguageDictionary(languageStore);

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