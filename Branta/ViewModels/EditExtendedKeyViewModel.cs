using Branta.Models;
using Branta.Stores;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Branta.ViewModels;

public partial class EditExtendedKeyViewModel : ObservableObject
{
    private readonly ExtendedKeyStore _extendedKeyStore;
    private readonly ExtendedKey _extendedKey;

    [ObservableProperty]
    private string _name;

    [ObservableProperty]
    private string _value;

	public Action CloseAction { get; set; }

    [RelayCommand]
    public void Submit()
    {
        if (_extendedKey == null)
        {
            _extendedKeyStore.Add(Name, Value);
        }
        else
        {
            _extendedKeyStore.Update(_extendedKey.Id, Name, Value);
        }

        CloseAction?.Invoke();
    }

    public EditExtendedKeyViewModel(ExtendedKeyStore extendedKeyStore, ExtendedKey extendedKey)
    {
        _extendedKeyStore = extendedKeyStore;
        _extendedKey = extendedKey;

        if (_extendedKey != null)
        {
            Name = _extendedKey.Name;
            Value = _extendedKey.Value;
        }
    }
}
