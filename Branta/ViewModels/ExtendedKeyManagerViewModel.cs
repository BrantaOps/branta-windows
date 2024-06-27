using Branta.Stores;
using Branta.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace Branta.ViewModels;

public partial class ExtendedKeyManagerViewModel : ObservableObject
{
    private readonly ExtendedKeyStore _extendedKeyStore;
    private readonly LanguageStore _languageStore;

    [ObservableProperty]
    private ObservableCollection<ExtendedKeyViewModel> _extendedKeys = [];

    [RelayCommand]
    public void OpenAdd()
    {
        var editExtendedKeyWindow = new EditExtendedKeyWindow(_extendedKeyStore, null, _languageStore);

        editExtendedKeyWindow.Show();
    }

    public ExtendedKeyManagerViewModel(ExtendedKeyStore extendedKeyStore, LanguageStore languageStore)
    {
        _extendedKeyStore = extendedKeyStore;
        _languageStore = languageStore;

        _extendedKeyStore.OnExtendedKeyUpdate += RefreshKeys;
        RefreshKeys();
    }

    public void RefreshKeys()
    {
        ExtendedKeys.Clear();

        foreach (var key in _extendedKeyStore.ExtendedKeys)
        {
            ExtendedKeys.Add(new ExtendedKeyViewModel(_extendedKeyStore, key, _languageStore));
        }
    }
}
