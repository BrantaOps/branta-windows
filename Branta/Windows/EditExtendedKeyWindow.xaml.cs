using Branta.Core.Data.Domain;
using Branta.Stores;
using Branta.ViewModels;

namespace Branta.Windows;

public partial class EditExtendedKeyWindow
{
    public EditExtendedKeyWindow(ExtendedKeyStore extendedKeyStore, ExtendedKey extendedKey, LanguageStore languageStore)
    {
        InitializeComponent();

        var viewModel = new EditExtendedKeyViewModel(extendedKeyStore, extendedKey, languageStore);
        DataContext = viewModel;

        viewModel.CloseAction += Close;
    }
}
