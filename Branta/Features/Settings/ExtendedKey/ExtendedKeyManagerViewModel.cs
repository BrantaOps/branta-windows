using Branta.Features.Main;
using Branta.Features.Settings.ExtendedKey;
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

    [ObservableProperty]
    private bool _isLoading;

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
        System.Windows.Application.Current.Dispatcher.Invoke(() =>
        {
            ExtendedKeys.Clear();

            foreach (var key in _extendedKeyStore.ExtendedKeys)
            {
                ExtendedKeys.Add(new ExtendedKeyViewModel(_extendedKeyStore, key, _languageStore));
            }

            IsLoading = _extendedKeyStore.IsLoading;
        });
    }
}
