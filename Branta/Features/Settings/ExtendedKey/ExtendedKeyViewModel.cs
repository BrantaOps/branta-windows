using Branta.Core.Data.Domain;
using Branta.Features.Main;
using Branta.Features.Settings.ExtendedKey;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;

namespace Branta.ViewModels;

public partial class ExtendedKeyViewModel : ObservableObject
{
    private readonly ExtendedKeyStore _extendedKeyStore;
    private readonly ExtendedKey _extendedKey;
    private readonly LanguageStore _languageStore;

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
        var editExtendedKeyWindow = new EditExtendedKeyWindow(_extendedKeyStore, _extendedKey, _languageStore);

        editExtendedKeyWindow.Show();
    }

    [RelayCommand]
    public async Task Remove()
    {
        var title = _languageStore.Format("MessageBox_ExtendedKey_ConfirmDeleteMessage", Name);
        var message = _languageStore.Get("MessageBox_ExtendedKey_ConfirmDeleteTitle");

        var result = MessageBox.Show(title, message, MessageBoxButton.YesNo);

        if (result == MessageBoxResult.Yes)
        {
            await _extendedKeyStore.RemoveAsync(_extendedKey.Id);
        }
    }

    public ExtendedKeyViewModel(ExtendedKeyStore extendedKeyStore, ExtendedKey extendedKey, LanguageStore languageStore)
    {
        _extendedKeyStore = extendedKeyStore;
        _extendedKey = extendedKey;
        _languageStore = languageStore;
    }
}
