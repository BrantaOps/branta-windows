using Branta.Models;
using Branta.Stores;
using Branta.ViewModels;

namespace Branta.Windows;

public partial class EditExtendedKeyWindow
{
    public EditExtendedKeyWindow(ExtendedKeyStore extendedKeyStore, ExtendedKey extendedKey)
    {
        InitializeComponent();

        var viewModel = new EditExtendedKeyViewModel(extendedKeyStore, extendedKey);
        DataContext = viewModel;

        viewModel.CloseAction += Close;
    }
}
