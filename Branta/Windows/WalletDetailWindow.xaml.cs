using Branta.Models;
using System.Windows;

namespace Branta.Views;

public partial class WalletDetailWindow
{
    private readonly ResourceDictionary _dictionary;

    public WalletDetailWindow(Wallet wallet)
    {
        InitializeComponent();

        _dictionary = SetLanguageDictionary();

        TbWallet.Text = $"{wallet.Name} {wallet.Version}";

        if (!_dictionary.Contains(wallet.Status.LanguageDictionaryName))
        {
            return;
        }

        var content = string.Format(_dictionary[wallet.Status.LanguageDictionaryName].ToString(), wallet.Name);

        IcMessages.ItemsSource = content.Split("_NL_").ToList();
    }
}