using Branta.Models;

namespace Branta.Windows;

public partial class WalletDetailWindow
{
    public WalletDetailWindow(Wallet wallet)
    {
        InitializeComponent();

        var dictionary = SetLanguageDictionary();

        TbWallet.Text = $"{wallet.Name} {wallet.Version}";

        if (!dictionary.Contains(wallet.Status.LanguageDictionaryName))
        {
            return;
        }

        var content = string.Format(dictionary[wallet.Status.LanguageDictionaryName]?.ToString() ?? string.Empty, wallet.Name);

        IcMessages.ItemsSource = content.Split("_NL_").ToList();
    }
}