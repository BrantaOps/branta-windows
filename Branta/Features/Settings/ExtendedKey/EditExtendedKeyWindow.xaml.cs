using Branta.Features.Main;
using Branta.ViewModels;

namespace Branta.Features.Settings.ExtendedKey;

public partial class EditExtendedKeyWindow
{
    public EditExtendedKeyWindow(ExtendedKeyStore extendedKeyStore, Core.Data.Domain.ExtendedKey extendedKey, LanguageStore languageStore)
    {
        InitializeComponent();

        var viewModel = new EditExtendedKeyViewModel(extendedKeyStore, extendedKey, languageStore);
        DataContext = viewModel;

        viewModel.CloseAction += Close;
    }
}
