using Branta.Models;
using Branta.Stores;
using Branta.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;

namespace Branta.ViewModels;

public partial class ExtendedKeyViewModel : ObservableObject
{
    private readonly ExtendedKeyStore _extendedKeyStore;
    private readonly ExtendedKey _extendedKey;

    public string Name => _extendedKey.Name;

    public string Value => _extendedKey.Value;

    [RelayCommand]
    public void Copy()
    {
        Clipboard.SetText(Value);
    }

    [RelayCommand]
    public void Edit()
    {
        var editExtendedKeyWindow = new EditExtendedKeyWindow(_extendedKeyStore, _extendedKey);

        editExtendedKeyWindow.Show();
    }


    [RelayCommand]
    public void Remove()
    {
        var result = MessageBox.Show($"Are you sure you want to delete '{Name}' xpub?", "Confirm Delete", MessageBoxButton.YesNo);

        if (result == MessageBoxResult.Yes)
        {
            _extendedKeyStore.Remove(_extendedKey.Id);
        }
    }

    public ExtendedKeyViewModel(ExtendedKeyStore extendedKeyStore, ExtendedKey extendedKey)
    {
        _extendedKeyStore = extendedKeyStore;
        _extendedKey = extendedKey;
    }
}
